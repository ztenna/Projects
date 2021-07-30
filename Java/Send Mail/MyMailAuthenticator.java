// Zachary Tennant

import javax.mail.PasswordAuthentication;

class MyMailAuthenticator extends javax.mail.Authenticator
{
	String authSenderUsername;
	String authSenderPassword;

	public MyMailAuthenticator(String authSenderUsername, String authSenderPassword)
	{
		this.authSenderUsername = authSenderUsername;
		this.authSenderPassword = authSenderPassword;
	} // end constructor

	protected PasswordAuthentication getPasswordAuthentication()
	{
		return new PasswordAuthentication(authSenderUsername, authSenderPassword);
	}
} // end class MyMailAuthenticator