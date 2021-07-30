// Dawn Sargent
// ImagePanel.java
// Last modified Marth 7, 2018
// JPanel for displaying an Image.

import javax.swing.*;
import java.util.*;
import java.awt.*;
import java.awt.image.*;

public class ImagePanel extends JPanel
{
   Image image;

   //================================================================
   ImagePanel( Image image )
   {
      this.image = image;

   } // end ImagePanel() constructor

   //================================================================
   @Override
   public void paintComponent( Graphics g )
   {
      Toolkit tk;

      tk = Toolkit.getDefaultToolkit();
      super.paintComponent( g );
      g.drawImage( image, 0, 0, this );

   } // end paintComponent()

} // end ImagePanel class