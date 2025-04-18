using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;

class Program
{
    static List<string> ReadFileToArray(string filePath, string separator)
    {
        if (!File.Exists(filePath))
        {
            return null;
            throw new FileNotFoundException("الملف غير موجود.");
        }

        // قراءة محتويات الملف بالكامل
        string fileContent = File.ReadAllText(filePath);

        // تقسيم النص باستخدام الفاصل المحدد
        string[] textArray = fileContent.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

        // إزالة الفراغات الإضافية من النصوص (اختياري)
        for (int i = 0; i < textArray.Length; i++)
        {
            textArray[i] = textArray[i].Trim();
        }

        return textArray.ToList<string>();
    }
    static void Main(string[] args)
    {
        string smtpConfigPath = "smtpConfig.txt"; // ملف إعدادات SMTP
        string smtpUsageLogPath = "SmtpUsageLog.txt"; // ملف سجل SMTP
        string emailSubjectsPath = "EmailSubjects.txt"; // ملف عناوين الإيميلات
        string emailBodiesPath = "EmailBodies.txt"; // ملف نصوص الإيميلات
        string recipientEmailsPath = "RecipientEmails.txt"; // ملف قائمة المستلمين
        string sentEmailsPath = "SentEmails.txt"; // ملف الإيميلات التي تم إرسالها

        var smtpConfigs = ReadSmtpConfigs(smtpConfigPath);
        var emailSubjects = ReadLines(emailSubjectsPath);
        var emailBodies = ReadFileToArray(emailBodiesPath,",,,");
        var recipientEmails = ReadLines(recipientEmailsPath);
        var sentEmails = ReadLines(sentEmailsPath).ToHashSet(); // الإيميلات المرسلة
        var smtpUsage = LoadSmtpUsage(smtpUsageLogPath);

        if (!smtpConfigs.Any() || !emailSubjects.Any() || !emailBodies.Any() || !recipientEmails.Any())
        {
            Console.WriteLine("تأكد من وجود البيانات في الملفات المطلوبة.");
            return;
        }

        recipientEmails = recipientEmails.Where(email => !sentEmails.Contains(email)).ToList(); // استبعاد الإيميلات المرسلة
        Random random = new Random();
        int smtpIndex = 0; // مؤشر SMTP

        while (recipientEmails.Any())
        {
            var currentSmtp = smtpConfigs[smtpIndex];

            if (!smtpUsage.ContainsKey(currentSmtp.Username))
                smtpUsage[currentSmtp.Username] = 0; // عداد جديد إذا لم يكن موجودًا

            int emailsSent = smtpUsage[currentSmtp.Username];

            if (emailsSent >= currentSmtp.MaxEmailsPerDay)
            {
                Console.WriteLine($"تم الوصول إلى الحد الأقصى لـ SMTP: {currentSmtp.Username}");
                smtpIndex = (smtpIndex + 1) % smtpConfigs.Count; // الانتقال إلى SMTP التالي
                continue;
            }

            var batch = recipientEmails.Take(currentSmtp.MaxEmailsPerBatch).ToList();
            if (!batch.Any()) break;

            try
            {
                string subject = emailSubjects[random.Next(emailSubjects.Count)];
                string body = emailBodies[random.Next(emailBodies.Count)];

                SendEmailBatch(currentSmtp, batch, subject, body);

                foreach (var recipient in batch)
                {
                    emailsSent++;
                    sentEmails.Add(recipient); // تسجيل الإيميل في قائمة المرسلة
                    AppendLineToFile(sentEmailsPath, recipient); // تسجيله في الملف
                }

                smtpUsage[currentSmtp.Username] = emailsSent; // تحديث عدد الرسائل المرسلة
                SaveSmtpUsage(smtpUsageLogPath, smtpUsage); // حفظ السجل

                recipientEmails = recipientEmails.Skip(batch.Count).ToList(); // إزالة الدفعة المرسلة
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطأ أثناء الإرسال: {ex.Message}");
            }

            smtpIndex = (smtpIndex + 1) % smtpConfigs.Count; // التبديل إلى SMTP التالي
            //Thread.Sleep(currentSmtp.DelayBetweenBatches * 60 * 1000); // الانتظار بين الدفعات
            Thread.Sleep(20000);
        }

        Console.WriteLine("تم الانتهاء من إرسال جميع الرسائل.");
    }

    static List<SmtpConfig> ReadSmtpConfigs(string path)
    {
        var configs = new List<SmtpConfig>();
        foreach (var line in File.ReadLines(path))
        {
            var parts = line.Split(',');
            if (parts.Length == 7)
            {
                configs.Add(new SmtpConfig
                {
                    Host = parts[0],
                    Port = int.Parse(parts[1]),
                    Username = parts[2],
                    Password = parts[3],
                    MaxEmailsPerBatch = int.Parse(parts[4]),
                    MaxEmailsPerDay = int.Parse(parts[5]),
                    DelayBetweenEmails = int.Parse(parts[6]),
                    DelayBetweenBatches = int.Parse(parts[6])
                });
            }
        }

        return configs;
    }

    static List<string> ReadLines(string path)
    {
        return File.Exists(path) ? File.ReadAllLines(path).ToList() : new List<string>();
    }

    static Dictionary<string, int> LoadSmtpUsage(string path)
    {
        var usage = new Dictionary<string, int>();
        if (File.Exists(path))
        {
            foreach (var line in File.ReadLines(path))
            {
                var parts = line.Split(',');
                if (parts.Length == 3)
                {
                    string smtpUsername = parts[0];
                    int usageCount = int.Parse(parts[1]);
                    usage[smtpUsername] = usageCount;
                }
            }
        }
        return usage;
    }

    static void SaveSmtpUsage(string path, Dictionary<string, int> usage)
    {
        using (var writer = new StreamWriter(path, false))
        {
            foreach (var entry in usage)
            {
                string logLine = $"{entry.Key},{entry.Value},{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                writer.WriteLine(logLine);
            }
        }
    }

    static void AppendLineToFile(string path, string line)
    {
        using (var writer = new StreamWriter(path, true))
        {
            writer.WriteLine(line);
        }
    }

    static void SendEmailBatch(SmtpConfig config, List<string> recipients, string subject, string body)
    {
        using (var client = new SmtpClient(config.Host, config.Port))
        {
            client.Credentials = new NetworkCredential(config.Username, config.Password);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(config.Username),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            foreach (var recipient in recipients)
            {
                try
                {
                    mailMessage.Bcc.Add(recipient); // الإضافة إلى قائمة BCC
                }
                catch { }
            }

            // client.Send(mailMessage);
            Console.WriteLine("------------------------");
            Console.WriteLine("Host: " + config.Host);
            Console.WriteLine("Port: " + config.Port);


            Console.WriteLine("Username: " + config.Username);
            Console.WriteLine("Password: " + config.Password);



            Console.WriteLine("------------------------");
            foreach (var recipient in recipients)
            {
                Console.WriteLine(recipient);
            }
            Console.WriteLine(subject);
            Console.WriteLine(body);

        }
    }
}

class SmtpConfig
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int MaxEmailsPerBatch { get; set; }
    public int MaxEmailsPerDay { get; set; }
    public int DelayBetweenEmails { get; set; }
    public int DelayBetweenBatches { get; set; }
}
