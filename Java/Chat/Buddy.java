// Zachary Tennant

class Buddy
{
	String username;
	String status;
	ChatBox chatBox;

	public Buddy(String username, String status, ChatBox chatBox)
	{
		this.username = username;
		this.status = status;
		this.chatBox = chatBox;
	} // end constructor

//===============================================

	public String toString()
	{
		String buddyString;

		buddyString = status + " " + username;
		return buddyString;
	}
} // end class Buddy