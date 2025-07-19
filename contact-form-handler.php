<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $name = htmlspecialchars($_POST['name']);
    $email = htmlspecialchars($_POST['email']);
    $message = htmlspecialchars($_POST['message']);

    echo "<h1>شكراً لك</h1>";
    echo "<p>لقد تم استلام رسالتك وسنقوم بالرد عليك قريباً.</p>";
    echo "<h2>تفاصيل الرسالة:</h2>";
    echo "<p><strong>الاسم:</strong> $name</p>";
    echo "<p><strong>البريد الإلكتروني:</strong> $email</p>";
    echo "<p><strong>الرسالة:</strong><br>$message</p>";
} else {
    echo "<h1>خطأ</h1>";
    echo "<p>حدث خطأ ما. يرجى المحاولة مرة أخرى.</p>";
}
?>
