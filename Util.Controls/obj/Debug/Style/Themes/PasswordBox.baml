   M S B A M L   `   `   `  ����H  DUtil.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Util.ControlsUtil.Controls  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType   DefaultPasswordBox �� Property  TextBoxContextMenu �� Value  TextSelectionBrush  
FontFamily  FontSize  TextBackground  ControlBorderBrush)    #Util.Controls.ControlAttachProperty  FocusBorderBrush  MouseOverBorderBrush �� 
TargetType �Name ��Name J� SnapsToDevicePixels    CornerRadius �� CornerRadius	 � ColumnDefinitions
 ��Name    LabelTemplate    Label ��Name ��Name �� Padding    	Watermark J� IsHitTestVisible 	 WatermarkOpacity J� Opacity    AttachContent �� Triggers �� Value �� Binding �� RelativeSource �� Path �� 
TargetName P� Property P� Value 
 DisableOpacity  ClearButtonPasswordBox �� BasedOn   Util.Controls.FButton   FIcon  FButton_Transparency   FIconMargin'     IsClearTextButtonBehaviorEnabled    	FIconSize  TextForeground! � Cursor" �� AncestorType  LabelPasswordBox  TextLabelBackground  LabelClearButtonPasswordBox  KeyPasswordBox# � UseLayoutRounding$ �� BitmapScalingMode  PwdPasswordBox�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      %localclr-namespace:Util.Controls  5      %Y  &	        7  7  7  7  7  7  7  7 	 7 
 &	 �    7   7  7  &	 .
    7   7  &	 1    7   7  7  7  &	 v    7   &	 �    7   �� 5      "  G�6   .�5   
   ��  �@�ContextMenu# �  6(   �� 5   
    �@G�SelectionBrush8   6+   �� 5	   
    � ��8  6'   �� 5
   
    � ��8  6%   �� 5   
    � ��$ 1q�6,   �� 5   
    �@G�PasswordChar$	 ●��6)   �� 5   
    � ��$ 30��6#   �� 5   
    � ��$	 200��6"   �� 5   
    � ��8  6'   �� 5   
    � �� �#3336'   �� 5   
    � ��$ 0q�6$   �� 5   
    � ��8  6(   �� 5   
    �@  FocusBorderBrush8  6I   �� 5   
    �@  MouseOverBorderBrush8  6M   �� 5   
    � ��$ Center=�65   �� 5   
    �@J�SnapsToDevicePixels . 60   �� 5   
    �@G�
CaretBrush �#3336'   �� 5   
    � �� 5      �� 5      " G�6"   .R�5      � $ 	PART_Root��6   .+�5      �� $ Bg��6!   # z 6-   # z 5   !   #��z��5    !   #��z��6M   #��z��6�   � 5!      $ PART_InnerGrid��6   	 5"      �� 5#   "   $
��Auto �64   �� 5$   "   $��* �63   �� 5%   "   $
��Auto �64   .+�5(      �� $
 Label��6-   $��1q�6<   #��z 6G   #��z 5)   -   �� 5+      $ PART_ContentHost��6+   $��0q�6E   $��1��6Y   ��.  $��2q�6{   $��Stretch=�5,   +   ��6G   Z� �� 5.      $ Message��6(   # z��6:   $y�	Collapsed=�6^   #��z 5/   (   $��1��6g   #��z��50   (    .  8  #��z��51   (   #��z��52   (   $��5,2,5,2q�6g   �� 55      $ PART_AttachContent��6%   $��2��6A   $��2q�6Q   $��Center=�6\   $��Center=�6w   ._�56   "   �� $��Center=�61   $��Center=�6L   #��z 6n   57      58      59       5:      �� 5<      $  ��6n    6&   ��  �� *Self+$ Password�.a�5=      �� $ Message��6%    � y�$ Visible=�6P   5>      P� 5?       �@J�IsMouseOver . . �5@      ��  6<   �� $8 2(local:ControlAttachProperty.MouseOverBorderBrush)� �� *Self+ � ��5A      P� 5B       �@J�	IsFocused . . �5C      ��  6=   �� $4 .(local:ControlAttachProperty.FocusBorderBrush)� �� *Self+ � ��5D      P� 5F       � {� .  . �5G      �� $ 	PART_Root��6%   8   �@J�Opacity6y   5H      5J      5L      5M      �� 5P      "  G�6   8   .�5Q   
   ��  �@  AttachContent 5R      �� 5S      .R�5T        $	 ��6$   8�� ��.  $ 0q�6u    . $2��,local:ControlAttachProperty.ClearTextCommand��5U   h   $��1,3,1,4q�5W       $  14��61   8�� $! Arrow��6m   ��5V   $   ��  �� *FindAncestor+"" G�5W   m   5X      5Z      5[      �� 5^      "  G�6   8   .�5_   
   ��  �@  LabelTemplate 5`      �� 5a      " ��6"   .R�5b      �� $��60��6   8�� ._�5c      �� $��Center=�6$   $��Right=�6?   $��3q�6[   #��z��6f   6�   5d      5e      5g      5h      �� 5k      "  G�6   8   .�5l   
   ��  �@  LabelTemplate 5m      �� 5n      " ��6"   .R�5o      �� $��60��6   8�� ._�5p      �� $��Center=�6$   $��Right=�6?   $��3q�6[   #��z��6f   6�   5q      5r      5t      �� 5u   
    �@  AttachContent 5v      �� 5w      .R�5x        $	 ��6$   8�� ��.  $ 0q�6u    . $2��,local:ControlAttachProperty.ClearTextCommand��5y   d   $��0,3,1,4q�5{       $  14��61   8�� $! Arrow��6m   ��5z       ��  �� *FindAncestor+"" G�5{   m   5|      5~      5      �� 5�      "  G�6   8   .�5�   
   ��  �@  LabelTemplate 5�      �� 5�      " ��6"   .R�5�      �� $
��None=�6   # . $$ NearestNeighbor=�6D   $��Center=�6v   $��	20,0,10,0q�6�   $5��//Util.Controls;component/Resources/icon-key.png��6�   6�   5�      5�      5�      �� 5�      "  G�6   8   .�5�   
   ��  �@  LabelTemplate 5�      �� 5�      " ��6"   .R�5�      �� $
��None=�6   # . $$ NearestNeighbor=�6D   $��Center=�6v   $��	20,0,10,0q�6�   $0��*/Util.Controls;component/Resources/pwd.png��6�   6�   5�      5�      5�      5�      