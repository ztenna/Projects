import javax.swing.table.*;
import javax.swing.*;
import java.awt.*;

public class Query3TableCellRenderer
       extends DefaultTableCellRenderer {
  public Component getTableCellRendererComponent(JTable table,
                                                 Object value,
                                                 boolean isSelected,
                                                 boolean hasFocus,
                                                 int row,
                                                 int column) {
    Component c =
      super.getTableCellRendererComponent(table, value,
                                          isSelected, hasFocus,
                                          row, column);

    // Only for specific cell
    if (column == 3) {
       // c.setFont(/* special font*/);
       // you may want to address isSelected here too
       c.setForeground(Color.GREEN);
    }

    else if (column == 4) {
       // c.setFont(/* special font*/);
       // you may want to address isSelected here too
       c.setForeground(Color.BLUE);
    }

    return c;
  }
}