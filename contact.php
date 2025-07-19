<?php include 'header.php'; ?>

<div class="container">
    <h1>اتصل بنا</h1>
    <p>يمكنك التواصل معنا بالاتصال على الرقم +905372851589 أو إرسال بريد إلكتروني على info@ghayatltd.com.</p>

    <form action="contact-form-handler.php" method="post">
        <label for="name">الاسم:</label>
        <input type="text" id="name" name="name" required>

        <label for="email">البريد الإلكتروني:</label>
        <input type="email" id="email" name="email" required>

        <label for="message">الرسالة:</label>
        <textarea id="message" name="message" required></textarea>

        <button type="submit">إرسال</button>
    </form>
</div>

<?php include 'footer.php'; ?>
