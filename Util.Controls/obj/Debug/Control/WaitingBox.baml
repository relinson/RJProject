   M S B A M L   `   `   `  ����  Util.Controls    System.Windows.WaitingBox   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          �Name$  wb��5   -    � AllowsTransparency .  � WindowStyle$
 None=�5   #    � WindowStartupLocation$ CenterOwner=�66    � ShowInTaskbar .  
������  � Title$ 
WaitingBox��5   	   $	��110��6   $	��260��6)   .��5      � .+�5	   
   ��    WaitingBoxBackground#��[  6    �� CornerRadius$ 4��6D    J� Opacity$	 0.8Q�6U     WindowDropShadow J� Effect# [ 6d   6�   �� 5
   
   $��Center=�6   	 �� Orientation$	 Vertical=�61   $��Center=�6I   $��5q�6f   .+�5      �� -   5        
 ��Name$
 FIcon��5   (   $	����6   $��50(�67    	 FIcon#��[ 6E    J� RenderTransformOrigin$ 0.5,0.5&�6e   $	��5,0q�6�   z�5      �� 5      -   5        
TransFIcon�� �� Angle$ 0Q�5   :   5      �� 5      -   5        $
 
txtMessage��5      
�������$
��0,10q�6?   $��Center=�6N   $
��Wrap=�6i   
Loading...6�   5      5       � Triggers 5      :� 5   
    :� RoutedEvent$ Window.Loaded��6   .[�5      �� .��5      �� .��5      W�  g� RepeatBehavior$ Forever��6&    �� 
TargetName$ 
TransFIcon��6?    �� TargetProperty$ Angle�5   &   	 W� To$	 360X�6H    g� Duration$ 0:0:2.5D�6Q   5      5      5      5      