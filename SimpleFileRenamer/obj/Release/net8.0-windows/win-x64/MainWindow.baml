   M S B A M L   `   `   `  ���� L  HSimpleFileRenamer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:SimpleFileRenamerSimpleFileRenamer  G(clr-namespace:SimpleFileRenamer.ControlsSimpleFileRenamer.Controls  "    SimpleFileRenamer.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:SimpleFileRenamer     =controls(clr-namespace:SimpleFileRenamer.Controls       � Title$  Simple File Renamer��$	��650��$	��900��$	��500��$	��800��   BackgroundBrush#��[   � WindowStartupLocation$ CenterScreen=�.��� $��12q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0,0,0,12q�.+��� $��Simple File Renamer��$��24(�$
��Bold"�  	TextBrush#��[ $��0,0,0,4q��� $*��$Rename multiple files using patterns��$��14(�  LightTextBrush#��[ $��0,0,0,12q���  �� Orientation$ 
Horizontal=�$��0,8,0,0q�.+��� -    ��Name$ AddFilesButton��$��	Add Files��$
��12,6q�$��0,0,8,0q��� -   $ AddFolderButton��$��
Add Folder��$
��12,6q�$��0,0,8,0q��� -   $ ClearListButton��$��
Clear List��$
��12,6q�$��0,0,8,0q����#7777774   .SimpleFileRenamer.Controls.PatternInputControl  -     Name$ PatternInput��4   .SimpleFileRenamer.Controls.PatternInputControl   PatternChanged$! PatternInput_PatternChanged��$��1��$��0,0,0,12q�0   *SimpleFileRenamer.Controls.FileListControl  -     Name$ FileList��$��2��$��0,0,0,12q��� $��3��
����������#CCCCCC$��1q� �� Padding$ 8q�$��0,0,0,12q�._��� -   	 ��Name$	 
StatusText��$��Ready��#��[ � $��4��
 � ColumnDefinitions
 �� $��* ��� $
��Auto �.+��� $��0��$ 
Horizontal=�.+��� -    ��Name$ CreateUndoFileCheckbox��$��Create undo file�� d� 	IsChecked$
 TrueY�$��Center=�$��0,0,12,0q��� $��1��$ 
Horizontal=�$��Right=�.+��� -   $ RenameFilesButton��$��Rename Files��$
��16,8q�$
��Bold"�  SecondaryBrush#��[ ${�False��