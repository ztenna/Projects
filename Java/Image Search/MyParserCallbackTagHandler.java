// Zachary Tennant

import javax.swing.*;
import javax.swing.text.*;
import javax.swing.text.html.*;
import java.util.*;

class MyParserCallbackTagHandler extends HTMLEditorKit.ParserCallback
{
	String baseDomain;
	DefaultListModel<String> list;

	public MyParserCallbackTagHandler(String baseDomain, DefaultListModel list)
	{
		this.baseDomain = baseDomain;
		this.list = list;
	}

//===============================================

	public void handleSimpleTag(HTML.Tag tag, MutableAttributeSet attSet, int pos)
	{
		Object attribute;
		Enumeration<?> attributeEnum;

		if(tag == HTML.Tag.IMG)
		{
			attribute = attSet.getAttribute(HTML.Attribute.SRC);
			if(attribute != null)
				list.addElement(attribute.toString());
		}
	} // end of handleSimpleTag(...)
}// end of class TagHandler