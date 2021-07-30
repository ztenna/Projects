// Zachary Tennant

import javax.swing.*;

class BuddyList extends DefaultListModel<Buddy>
{
	public BuddyList()
	{
	}

//===============================================

	Buddy get(String usernameToFind)
	{
		int n = 0;
		boolean found = false;

		while(!found && n < size())
		{
			if(elementAt(n).username.equals(usernameToFind))
				found = true;
			else
				n = n + 1;
		}
		if(found)
			return elementAt(n);
		else
			return null;
	}
} // end class BuddyList