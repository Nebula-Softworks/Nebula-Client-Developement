   M S B A M L   `   `   `  ����R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   ScrollBarButton �� 
TargetType �� Property �� Value �� 
TargetType ��Name  ScrollBar.Static.Background J� SnapsToDevicePixels ��Name	 �� Triggers
 P� Property P� Value �� 
TargetName " ScrollBar.MouseOver.Background   ScrollBar.Pressed.Background ! ScrollBar.Disabled.Background  RepeatButtonTransparent  ScrollBarThumbVertical  ScrollBar.Static.Thumb �� CornerRadius  ScrollBar.MouseOver.Thumb 	 ScrollBar.Pressed.Thumb 
 ScrollBarThumbHorizontal $  SystemColors.ControlTextBrushKey �Name � RowDefinitions ]�Name J� IsMouseOver ]� DecreaseRepeatButton �� PageUpCommand ]� IncreaseRepeatButton �� PageDownCommand ]� Thumb ^� 
Conditions �� Binding �� ElementName �� Value �� Triggers � ColumnDefinitions �� PageLeftCommand �� PageRightCommand ��Name  ��Name! �� CanContentScroll" �� CanHorizontallyScroll# �� CanVerticallyScroll$ ��Name4   .System.Windows.Automation.AutomationProperties%    AutomationId& � Cursor' �� ScrollableHeight( � Maximum) � Minimum* �� ViewportHeight+ �� ViewportSize*, �� #ComputedVerticalScrollBarVisibility- � Value. �� Mode/ �� RelativeSource0 �� ScrollableWidth1 �� Orientation2 �� ViewportWidth,3 �� %ComputedHorizontalScrollBarVisibility�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      5        5      �� 5   
   $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml?�6   �� 5   
   $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6   �� 5   
   $" /Graphics/Styles/Colors.xaml?�6   %�  &	        7  7  7  7  &	 L    &	 �    7  7  7 	 &	
 �    7  7  7 	 '�� �    7  7  7  7  7  7  7 
 '�� �    7  �� 5
      " ��6$   .�5   
   ��  � ��$ Center=�67   �� 5   
    � ��$ Center=�65   �� 5   
    � ��$ 1q�6$   �� 5   
    � |� .  6&   �� 5   
    � �� .  6&   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      �� $ border��6   8��   . ._�5      �� $ contentPresenter��5   %   #��z��5   %   #��z��5   %   #��z��5   %   |�.  # z 5   %   5      	 5      P� 5      
 �@J�IsMouseOver . . �5       �� $ border��6%   8   � ��6O   5!      P� 5"      
 � �� . . �5#      �� $ border��6%   8   � ��6O   5$      P� 5%      
 � {� .  . �5&      �� $ contentPresenter��6%    �@J�Opacity$
 0.56Q�6V   �� 5'      $ border��6%   8   � ��6O   5(      5*      5,      5-      �� 5.      " ��6,   .�5/   
   ��  �@�OverridesDefaultStyle . 62   �� 50   
    � �� �	#00FFFFFF6'   �� 51   
    � |� .  6&   �� 52   
    � �� .  6&   �� 53   
    � �� 54      �� 55      " ��6"   .R�56      �� #��z��6    #��z��57       #��z��58       59      5;      5<      �� 5>      " n�6+   .�5?   
   ��  �@�OverridesDefaultStyle . 62   �� 5@   
    � �� .  6&   �� 5A   
    � �� 5B      �� 5C      " n�6"   .R�5D      �� $ 	rectangle��6   #��z��5E      #��z��5F      8��  $ 3��5H       . 5I      	 5J      P� 5K      
 �@J�IsMouseOver . . �5L      �� $ 	rectangle��6%   8   � ��6R   5M      P� 5N      
 �@n�
IsDragging . . �5O      �� $ 	rectangle��6%   8   � ��6R   5P      5R      5T      5U      �� 5V      " n�6-   .�5W   
   ��  �@�OverridesDefaultStyle . 62   �� 5X   
    � �� .  6&   �� 5Y   
    � �� 5Z      �� 5[      " n�6"   .R�5\      �� $ 	rectangle��6   #��z��5]      $��5��5^      8��  $	 1.5��5`       . 5a      	 5b      P� 5c      
 �@J�IsMouseOver . . �5d      �� $ 	rectangle��6%   8   � ��6R   5e      P� 5f      
 �@n�
IsDragging . . �5g      �� $ 	rectangle��6%   8   � ��6R   5h      5j      5l      5m      �� 5n      " ��6'   .�5o   
   ��  �@��IsPressAndHoldEnabled .  69   �� 5p   
    �@��IsFlicksEnabled .  63   �� 5q   
    � ��8   6'   �� 5r   
    � ��# �  6'   �� 5s   
    � ��$ 8��6"   �� 5t   
    � ��$ 8��6%   �� 5u   
    � �� 5v      �� 5w      " ��6"   .R�5x      � $ Bg��6    .  5y      �� 5z      $��0.00001* �6,   .+�5|      �� $��0��6!   #��z��6.   ]� 5}      $ 
PART_Track��6    $��0��5~       ��. #{�z 5�        5�      �� 5�   "   #��Z 6/   8�� 6\    5�      �� 5�   "   #��Z 6/   8�� 6^    5�      n� 5�   "   8�� 6(   5�      5�      	 5�      ^� 5�       5�      �� 5�   "    6,   �� *IsMouseOver+$ PART_LineDownButton�� true6m   �� 5�   "    6,   �� *	IsPressed+$ PART_LineDownButton�� true6k   5�      ^� 5�       5�      �� 5�   "    6,   �� *IsMouseOver+$ PART_LineUpButton�� true6k   �� 5�   "    6,   �� *	IsPressed+$ PART_LineUpButton�� true6i   5�      ^� 5�       5�      �� 5�   "    6,   �� *IsMouseOver+$ PART_LineDownButton�� true6m   �� 5�   "    6,   �� *	IsPressed+$ PART_LineDownButton��	 false6k   5�      ^� 5�       5�      �� 5�   "    6,   �� *IsMouseOver+$ PART_LineUpButton�� true6k   �� 5�   "    6,   �� *	IsPressed+$ PART_LineUpButton��	 false6i   5�      P� 5�      
 � {� .  67   5�      5�       5�   
   P� 5�      
 �@��Orientation$ 
Horizontal=�6-   . �5�      ��  � ��$
 Auto��6*   �� 5�       � ��$ 0��6-   �� 5�       � ��$ 8��6+   �� 5�       � ��$ 8��6.   �� 5�       � �� 5�      �� 5�      " ��6*   .R�5�      � $ Bg��6#    .  5�   "   �� 5�   &   $��0.00001* �67   .+�5�   "   �� $��0��6)   #��z��69   ]� 5�   "   $ 
PART_Track��6(   $��0��5�   (   #{�z 5�   (    5�   &   �� 5�   *   #��Z 67   8�� 6f    5�   &   �� 5�   *   #��Z 67   8�� 6g    5�   &   n� 5�   *   8�� 60   5�   #   5�      	 5�      ^� 5�   "    5�   &   �� 5�   *    64   �� *IsMouseOver+$ PART_LineRightButton�� true6v   �� 5�   *    64   �� *	IsPressed+$ PART_LineRightButton�� true6t   5�   #   ^� 5�   "    5�   &   �� 5�   *    64   �� *IsMouseOver+$ PART_LineLeftButton�� true6u   �� 5�   *    64   �� *	IsPressed+$ PART_LineLeftButton�� true6s   5�   #   ^� 5�   "    5�   &   �� 5�   *    64   �� *IsMouseOver+$ PART_LineRightButton�� true6v   �� 5�   *    64   �� *	IsPressed+$ PART_LineRightButton��	 false6t   5�   #   ^� 5�   "    5�   &   �� 5�   *    64   �� *IsMouseOver+$ PART_LineLeftButton�� true6u   �� 5�   *    64   �� *	IsPressed+$ PART_LineLeftButton��	 false6s   5�   #   P� 5�   "   
 � {� .  6?   5�      5�      5�      5�      �� 5�      " ��6*   .�5�   
   ��  � �� 5�      �� 5�      " ��6"   .R�5�      � $
 Grid��6   #��z��6)    5�      �� 5�      $��* �6/   �� 5�      $
��Auto �6/    5�      �� 5�      $��* �6,   �� 5�      $
��Auto �6,   .+�5�      �� $ Corner��5�   %   $��1��5�   %   $��1��5�   %   8��  5�   %   �� 5�      $!  PART_ScrollContentPresenter��5�   %   $��0��5�   %   $��0��5�   %   #��z��5�   %   #! z��5�   %   " .  # .  #��z��5  %   #��z��5  %   �� 5     $$ PART_VerticalScrollBar��5  %   $��0��5  %   $��1��5  %   $% VerticalScrollBar��5  %   $& Arrow��5  %   #( z' 5	  %   $) 0Q�5
  %   #+ z* 5  %   #y�z, 5  %   - 5  %   �� *VerticalOffset+$. OneWay=�/ �� *TemplatedParent+�� 5     $$ PART_HorizontalScrollBar��5  %   $��1��5  %   $��0��5  %   $% HorizontalScrollBar��5  %   $& Arrow��5  %   #( z0 5  %   $) 0Q�5  %   $1 
Horizontal=�5  %   #+ z2 5  %   #y�z3 5  %   - 5  %   �� *HorizontalOffset+$. OneWay=�/ �� *TemplatedParent+5     5     5     5     5     