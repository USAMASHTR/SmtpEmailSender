💬 Bulk Email Sender in C#
About
This is a C# console application for sending bulk emails using multiple SMTP configurations. It supports batch-based email sending, randomized subject/body selection, logging of sent emails, and respects per-day sending limits.

How It Works
SMTP configurations are read from smtpConfig.txt

Emails are sent in batches, observing the daily and batch limits per SMTP

Subjects and bodies are selected randomly from EmailSubjects.txt and EmailBodies.txt

Sent emails are logged in SentEmails.txt to avoid duplicates

Email usage per SMTP is tracked in SmtpUsageLog.txt

Required Files
smtpConfig.txt – SMTP details in the format:
Host,Port,Username,Password,MaxPerBatch,MaxPerDay,Delay

EmailSubjects.txt – One subject per line

EmailBodies.txt – Email bodies separated by ,,,

RecipientEmails.txt – List of recipient emails, one per line

SentEmails.txt – Automatically updated list of sent emails

SmtpUsageLog.txt – Automatically created to track daily usage

Notes
Emails are added to BCC to send one batch at once.

Actual sending is commented out (client.Send() is not active).

There is a fixed 20-second delay between batches (adjustable in the code).


<a href="https://ghayatltd.com/ar/limited-liability-company-llc/" >تاسيس شركه في امريكا </a>

ghayat ltd
👉 غايات
<a href="https://ghayatltd.com/ar" >غايات </a>

📨 برنامج إرسال رسائل بريد إلكتروني جماعية باستخدام C#
نظرة عامة
هذا المشروع عبارة عن سكربت مكتوب بلغة #C لإرسال رسائل بريد إلكتروني جماعية إلى قائمة من المستلمين باستخدام عدة حسابات SMTP.
يعتمد البرنامج على ملفات نصية خارجية لتخزين الإعدادات والمحتوى، ويقوم بتوزيع الرسائل على دفعات بناءً على الإعدادات المحددة لكل SMTP.

📂 الملفات المطلوبة
قبل تشغيل البرنامج، تأكد من وجود الملفات التالية في نفس مجلد التشغيل:


اسم الملف	الوصف
smtpConfig.txt	يحتوي على إعدادات SMTP (كل سطر يمثل إعدادات حساب واحد).
SmtpUsageLog.txt	سجل بعدد الرسائل التي أُرسلت باستخدام كل حساب SMTP.
EmailSubjects.txt	قائمة عناوين الرسائل (كل سطر عنوان).
EmailBodies.txt	قائمة نصوص الرسائل، مفصولة بـ ,,, (مناسبة لإرسال رسائل مختلفة).
RecipientEmails.txt	قائمة عناوين البريد الإلكتروني للمستلمين (كل سطر بريد).
SentEmails.txt	قائمة الإيميلات التي تم إرسال الرسائل إليها (يتم تحديثها تلقائيًا).
🛠 طريقة إعداد الملفات
smtpConfig.txt
كل سطر يمثل إعدادات حساب SMTP بالترتيب التالي:

pgsql
نسخ
تحرير
Host,Port,Username,Password,MaxEmailsPerBatch,MaxEmailsPerDay,DelayInMinutes
مثال:

css
نسخ
تحرير
smtp.gmail.com,587,myemail@gmail.com,password123,10,100,1
💡 طريقة العمل
يتم قراءة جميع الإعدادات من الملفات الخارجية.

يتم تجاهل المستلمين الذين تم إرسال رسائل إليهم مسبقًا.

يتم استخدام حسابات SMTP بالتناوب حتى لا يتم تجاوز الحد اليومي المحدد.

يتم إرسال الرسائل على دفعات، وتسجيل المرسلة لتجنب التكرار.

كل دفعة تتأخر قليلًا (مثبتة حاليًا على 20 ثانية داخل الكود).

⚙️ مميزات السكربت
دعم متعدد لحسابات SMTP.

تخصيص عدد الرسائل لكل دفعة ولكل يوم.

إمكانية تدوير النصوص والعناوين بشكل عشوائي.

تسجيل تلقائي للإيميلات التي تم إرسالها.

سهل التعديل والتخصيص.

🧪 ملاحظات تقنية
لم يتم تفعيل إرسال الرسائل فعليًا (سطر client.Send(mailMessage) معطّل لغرض التجربة).

البرنامج يعرض إعدادات SMTP والرسالة في الكونسول كنوع من التصحيح (debug).

عند التشغيل الفعلي، يُنصح بإزالة طباعة كلمات المرور من الكونسول.

🖥️ مثال تشغيل
bash
نسخ
تحرير
dotnet run
📌 تنبيه
تأكد من عدم تجاوز الحد المسموح به من قبل مزود SMTP الخاص بك.

استخدم حسابات تجريبية لتجربة الإرسال قبل التشغيل الفعلي.

👨‍💻 الكود من تطوير
هذا السكربت قابل للتعديل والاستخدام في المشاريع التجارية أو الشخصية، ويمكنك تطويره أكثر ليشمل:

دعم المرفقات.

واجهة رسومية (GUI).

جدولة تلقائية للإرسال.




