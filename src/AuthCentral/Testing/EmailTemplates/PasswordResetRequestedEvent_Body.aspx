<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>Demystifying Email Design</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
</head>
<body>
<h1>This is a test template!</h1>
<h3>Reset your password</h3>
Hello {username}!

<p>
    You are getting this email because either you (or someone else) has requested to reset your password for {applicationName}.  If you would like to reset your password, please click on <a href="{confirmPasswordResetUrl}">this link!</a>
</p>
<p>
    If you did not request this password reset, please <a href="{cancelVerificationUrl}">click here</a> to cancel the request.
</p>

<p>Thanks!</p>
{emailSignature}
</body>
</html>