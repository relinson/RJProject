   M S B A M L   `   `   `  ����H  DUtil.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Util.ControlsUtil.Controls  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source �� 
TargetType   ComboBoxItemStyle �� Property �� Value �� 
TargetType ��Name J� IsHitTestVisible J� SnapsToDevicePixels	 ��Name
 �� Triggers P� Property P� Value �� 
TargetName  ItemSelectedBackground  ItemMouseOverBackground  DefaultComboBox  TextBackground  ControlBorderBrush)    #Util.Controls.ControlAttachProperty  FocusBorderBrush  MouseOverBorderBrush  
FontFamily 	 FontSize �Name    CornerRadius �� CornerRadius � ColumnDefinitions ��Name    LabelTemplate    Label d�Name 
 ComboToggleButton    FocusBackground d� 	IsChecked �� Mode �� RelativeSource �� Path   Util.Controls.XConverter   TrueToFalseConverter �� 	Converter ��Name �� Padding    	Watermark  WatermarkOpacity  J� Opacity! �� TextAlignment" �� SelectionBoxItemTemplate# �� SelectionBoxItem#$ �� SelectionBoxItemStringFormat% �� ContentStringFormat& ��Name  EditableTextBoxStyle' �� 
IsReadOnly( �� 
IsReadOnly) �� 
CaretBrush* �� Text+ �Name, � AllowsTransparency  PopupBackground- ��Name. ��Name-  'System.Windows.Input.KeyboardNavigation/   DirectionalNavigation0 �� Value1 �� Binding  DisableOpacity  ClearButtonComboBox2 �� BasedOn   Util.Controls.FButton3   FIcon  FButton_Transparency4   FIconMargin'5     IsClearTextButtonBehaviorEnabled6   	FIconSize  TextForeground7 � Cursor8 �� AncestorType  LabelComboBox  TextLabelBackground  LabelClearButtonComboBox  TypeCombobox9 � UseLayoutRounding: �� BitmapScalingMode  TaxCombobox�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    %localclr-namespace:Util.Controls  5      :x,http://schemas.microsoft.com/winfx/2006/xaml     5        5      �� 5   
   $L Fpack://application:,,,/Util.Controls;component/Style/Themes/Share.xaml?�6   %2  &	        7  7  &	 �    7  7  7  7  7  7 	 7   7 
 7  7  7  7  &	 �    7  7  7  &	 B    7  7  &	 o    7  7  7  7  &	 4    7  &	 ^    7  �� 5	      " ��6   .�5
   
   ��  � ��$
 Left=�67   �� 5   
    � ��$ Center=�65   �� 5   
    �@��ClearTypeHint$ Enabled=�68   �� 5   
    � ��$ 0q�6,   �� 5   
    � ��$ 28��6#   �� 5   
    � �� �#33366   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      � #��z��6   $��0,0.5q�6E   .+�5      �� $ ItemBackground��6!    .  # z 6R   #��z��5   !   #��z��6U   #��z��5   !   �� 5      $	 contentPresenter��6,   #��z��6F   #��z��6i   #��z��5   +   5      
 5      P� 5       �@��
IsSelected . . �5      �� $ ItemBackground��6%   8    � ��6W   �� 5       � ��
 �����6<   5      P� 5       �@J�IsMouseOver . . �5       �� $ ItemBackground��6%   8   � ��6W   �� 5!       � ��
 �����6<   5"      5$      5&      5'      �� 50      " ��6   .�51   
   ��  � ��$ 30��6#   �� 52   
    � ��$	 200��6"   �� 53   
    � �� �#3336'   �� 54   
    � ��8   6'   �� 55   
    � ��$ Stretch=�67   �� 56   
    � ��$ Center=�65   �� 57   
    � ��$ 1q�6,   �� 58   
    � ��8  6(   �� 59   
    �@  FocusBackground
 �����6X   �� 5:   
    �@  FocusBorderBrush8  6I   �� 5;   
    �@  MouseOverBorderBrush8  6M   �� 5<   
    � ��8  6'   �� 5=   
    � ��8  6%   �� 5>   
    � ��8  6/   �� 5?   
    �@J�SnapsToDevicePixels . 60   �� 5A   
    �@��MaxDropDownHeight$	 400��6.   �� 5B   
    � �� .  6:   �� 5C   
    � �� 5D      �� 5E      " ��6"   .R�5F      � $ 	PART_Root��6   .+�5G      �� $ Bg��6!   # z 6-   # z 5H   !   #��z��5I   !   #��z��6M   #��z��6�   � 5J       5K      �� 5L   "   $
��Auto �63   �� 5M   "   $��* �63   �� 5N   "   $
��Auto �63   �� 5O   "   $��21 �63   .+�5R      �� $ Label��6,   #��z 6;   ��.   .  #��z 5S   -   $��1,1,0,1q�6k   d� 5Y      $ PART_DropDownToggle��6+   ��.  8�� $��2,1,6,1q�5Z   �   $��1��5[   *   $��3��6;   #��z 5]   +    5Z   *   �� *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+{�5[   O   �� $ 
IsReadOnly� �� *TemplatedParent+# Z $ OneWay=�5]   +   �� 5_      $��1��6%   ._�5`   "   �� $ Message��6,   $ 0q�6>   $y�	Collapsed=�6J   #��z 6a   #��z��5a   (    .  8   $
��Left=�6�   $! Center=�6�   $��Center=�5b   (   $��5,2,5,2q�6C   5c      � 5e      $ ContentSite��63   $��1��6#   $��2 0 0 0q�6H   .+�5f   "   �� $	 PART_SelectedItem��63   #��z" 6N   #��z��5g   3   #��z# 5h   3   #% z$ 6`   #��z��5i   3   $��2,0,2,0q�6v    .  # z 5j   L   #��z��5k   3   �� 5m   "   $& PART_EditableTextBox��6+   8��	 $y�	Collapsed=�6w   $��Stretch=�5n   *   # z 6H    . #��z��5o   *   #��z��6t   #( z' 5p   *   #��z��6T   #��z��6~   ) �#333#��z* 5q   *   #��z��6J   5r      5s      � 5u      $+ 
PART_Popup��6    , . |�.  #��� .�5w       $��Bottom=�6y   ��5v       �� *IsDropDownOpen+ �� *TemplatedParent+.��5x      � ��6#   �� *ActualWidth+ �� *TemplatedParent+��5y   #   �� *MaxDropDownHeight+ �� *TemplatedParent+.+�5z   "   �� $ PopupBorder��6)   #��z��6>   $��Stretch=�6r   $
��Auto��5{   )   #��z��67   8��
 5}   #   �� 5~   "   $- DropDownScrollViewer��6/   $��0q�6M   $��1q�6a   .��5   &   �� $. ItemsPresenter��65   $/ 	Contained=�6M   5�   #   5�      5�      5�      
 5�      �� 5�      $0  ��6j   1 6&   ��  �� *Self+$
 Text�.a�5�      �� $ Message��6%    � y�$ Visible=�6P   5�      P� 5�       �@��
IsEditable . . �5�      �� $ PART_EditableTextBox��6%    � y�$ Visible=�6]   �� 5�      $ PART_EditableTextBox��6%    �@��
IsReadOnly .  6]   �� 5�      $ PART_SelectedItem��6%    � y�$ 	Collapsed=�6Z   �� 5�      $ PART_DropDownToggle��6%    � ��$ 3��6]   �� 5�      $ PART_DropDownToggle��6%    � ��$ 1��6a   �� 5�      $ PART_DropDownToggle��6%    � ��
 ���� 6\   �� 5�       � �� .  6:   �� 5�      $ PART_DropDownToggle��6%    � |� .  6[   5�      P� 5�       �@J�IsMouseOver . . �5�      ��  6<   �� $8 2(local:ControlAttachProperty.MouseOverBorderBrush)� �� *Self+ � ��5�      P� 5�       �@J�	IsFocused . . �5�      ��  6=   �� $4 .(local:ControlAttachProperty.FocusBorderBrush)� �� *Self+ � ��5�      P� 5�       �@J�IsKeyboardFocusWithin . . �5�      ��  6=   �� $4 .(local:ControlAttachProperty.FocusBorderBrush)� �� *Self+ � ��5�      P� 5�       � {� .  . �5�      �� $ 	PART_Root��6%   8   �@J�Opacity6y   5�      5�      5�      5�      �� 5�      " ��6   82   .�5�   
   ��  �@  AttachContent 5�      �� 5�      .R�5�        $	3 ��6$   8�� ��.  $4 0q�6u   5 . $2��,local:ControlAttachProperty.ClearTextCommand��5�   h   $��1,3,0,4q�5�       $6 14��61   8�� $7 Arrow��6m   ��5�   $   ��  �� *FindAncestor+"8 ��{�5�   $   �� $ 
IsReadOnly� �� *FindAncestor+"8 ��# Z $ OneWay=�5�   m   5�      5�      5�      �� 5�      " ��6   82   .�5�   
   ��  � ��$	 260��6"   60   �� 5�   
    �@  LabelTemplate 5�      �� 5�      " ��6"   .R�5�      �� $��60��6   8�� ._�5�      �� $��Center=�6$   $��Right=�6?   $��2q�6[   #��z��6f   6�   5�      5�      5�      5�      �� 5�      " ��6   82   .�5�   
   ��  � ��$	 280��6"   60   �� 5�   
    �@  LabelTemplate 5�      �� 5�      " ��6"   .R�5�      �� $��60��6   8�� ._�5�      �� $��Center=�6$   $��Right=�6?   $��2q�6[   #��z��6f   6�   5�      5�      5�      �� 5�   
    �@  AttachContent 5�      �� 5�      .R�5�        $	3 ��6$   8�� ��.  $4 0q�6u   5 . $2��,local:ControlAttachProperty.ClearTextCommand��5�   d   $��0,3,0,4q�5�       $6 14��61   8�� $7 Arrow��6m   ��5�       ��  �� *FindAncestor+"8 ��{�5�   $   �� $ 
IsReadOnly� �� *FindAncestor+"8 ��# Z $ OneWay=�5�   m   5�      5�      5�      �� 5�      " ��6   82   .�5�   
   ��  �@  LabelTemplate 5�      �� 5�      " ��6"   .R�5�      �� $
��None=�6   9 . $: NearestNeighbor=�6D   $��Center=�6v   $��	20,0,10,0q�6�   $1��+/Util.Controls;component/Resources/area.png��6�   6�   5�      5�      5�      �� 5�      " ��6   82   .�5�   
   ��  �@  LabelTemplate 5�      �� 5�      " ��6"   .R�5�      �� $
��None=�6   9 . $: NearestNeighbor=�6D   $��Center=�6v   $��	20,0,10,0q�6�   $1��+/Util.Controls;component/Resources/user.png��6�   6�   5�      5�      5�      5�      