   M S B A M L   `   `   `  ����L  HSimpleFileRenamer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:SimpleFileRenamerSimpleFileRenamer  5   	   G(clr-namespace:SimpleFileRenamer.ControlsSimpleFileRenamer.Controls  5   	   "    SimpleFileRenamer.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:SimpleFileRenamer     5   	   =controls(clr-namespace:SimpleFileRenamer.Controls     5   	     � Title$  Simple File Renamer��5	   	   $	��650��5
   	   $	��900��5   	   $	��500��5   	   $	��800��5   	      BackgroundBrush#��[  5   	    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��12q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0,0,0,12q�6"   .+�5      �� $��Simple File Renamer��6   $��24(�5      $
��Bold"�5        	TextBrush#��[ 5      $��0,0,0,4q�5       �� 5!      $*��$Rename multiple files using patterns��6   $��14(�5"        LightTextBrush#��[ 5#      $��0,0,0,12q�5$      � 5'      $��0,8,0,0q�6    � ColumnDefinitions 5(      �� 5)      $��* �6'   �� 5*      $
��Auto �6'   .+�5-      �� $��0��6    �� Orientation$ 
Horizontal=�5.      .+�5/      �� -   5         ��Name$ AddFilesButton��5/      $��	Add Files��50      $
��12,6q�51      $��0,0,8,0q�52      53      �� 54      -   5        $ AddFolderButton��54      $��
Add Folder��55      $
��12,6q�56      $��0,0,8,0q�57      58      �� 59      -   5        $ ClearListButton��59      $��
Clear List��5:      $
��12,6q�5;      $��0,0,8,0q�5<      ���#7777775>      5?      �� 5A      -   5        $ SettingsButton��5B      $��1��5A      $��Settings��5C      $
��12,6q�5D      $��Right=�5E      5F      5G      5H      4   .SimpleFileRenamer.Controls.PatternInputControl  5K   
   -   5          Name$ PatternInput��5K   '   4   .SimpleFileRenamer.Controls.PatternInputControl   PatternChanged$! PatternInput_PatternChanged��5N   &   $��1��5L   &   $��0,0,0,12q�5M   &   5N   &   0   *SimpleFileRenamer.Controls.FileListControl  5Q   
   -   5          Name$ FileList��5Q   #   $��2��5R   #   $��0,0,0,12q�5S   #   �� 5V   
   $��3��6     ControlBackgroundBrush#��[ 5W        BorderBrush#��[ 5X      $��1q�5Y      	 �� Padding$	 8q�5Z      $��0,0,0,12q�5[      ._�5\      �� -   5        
 ��Name$
 
StatusText��5\      $��Ready��5]      #��[ 5^      5_      � 5b   
   $��4��6    5c      �� 5d      $��* �6#   �� 5e      $
��Auto �6#   .+�5h      �� $��0��6   $ 
Horizontal=�5i      .+�5j      �� -   5         ��Name$ CreateUndoFileCheckbox��5j      $��Create undo file��5k       d� 	IsChecked$
 TrueY�5l      $��Center=�5m      $��0,0,12,0q�5n      5o      �� 5q      $��1��6   $ 
Horizontal=�5r      $��Right=�5s      .+�5t      �� -	   5        $ RenameFilesButton��5t      $��Rename Files��5u      $
��16,8q�5v      $
��Bold"�5w        SecondaryBrush#��[ 5x      ${�False��5z      5{      5|      5}      5~      