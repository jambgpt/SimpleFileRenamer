   M S B A M L   `   `   `  ���� L  HSimpleFileRenamer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:SimpleFileRenamer.ControlsSimpleFileRenamer.Controls  4    .SimpleFileRenamer.Controls.PatternInputControl   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :local(clr-namespace:SimpleFileRenamer.Controls     .���� 
����������#CCCCCC$��1q�  �� Padding$  12q�._��  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� $��0�� � ColumnDefinitions �� $
��Auto ��� $��* ��� $
��Auto ��� $��* �.+��� $��0��$��Prefix:��$��Center=�$��0,0,8,0q��� -    ��Name$ PrefixTextBox��$��1��$��0,0,16,0q��� $��2��$��Suffix:��$��Center=�$��0,0,8,0q��� -   $ SuffixTextBox��$��3��� $��1��$��0,12,0,0q� �� $
��Auto ��� $��* ��� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��Find:��$��Center=�$��0,0,8,0q��� -   $ FindTextBox��$��1��$��0,0,16,0q��� $��2��$��Replace:��$��Center=�$��0,0,8,0q��� -   $ ReplaceTextBox��$��3��$��0,0,16,0q��� -    ��Name$ UseRegexCheckBox��$��4��$��	Use Regex��$��Center=�� $��2��$��0,12,0,0q� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* �.+��� -   $ UseSequenceCheckBox��$��0��$��Add Sequence��$��Center=�$��0,0,16,0q��� $��1��$��Start:��$��Center=�$��0,0,8,0q�{���  �� ElementName$ UseSequenceCheckBox�� �� Path$ 	IsChecked��� -   $ SequenceStartTextBox��$��2��$��60��$��0,0,16,0q�$��1��{��� $ UseSequenceCheckBox��$ 	IsChecked��� $��3��$��
Increment:��$��Center=�$��0,0,8,0q�{��� $ UseSequenceCheckBox��$ 	IsChecked��� -   $ SequenceIncrementTextBox��$��4��$��60��$��0,0,16,0q�$��1��{��� $ UseSequenceCheckBox��$ 	IsChecked��� $��5��$��Format:��$��Center=�$��0,0,8,0q�{��� $ UseSequenceCheckBox��$ 	IsChecked��� -	   $ SequenceFormatTextBox��$��6��$��60��$��0,0,16,0q�$	��000��{��� $ UseSequenceCheckBox��$ 	IsChecked��� -
    ��Name$ SequencePositionComboBox��$��7��$��0,0,0,0q�{��� $ UseSequenceCheckBox��$ 	IsChecked�.@��� $��Add as Prefix��
 � Tag$ Prefix��	 �� 
IsSelected$
	 True���� $��Add as Suffix��$ Suffix���� $��Replace Filename��$ Replace��