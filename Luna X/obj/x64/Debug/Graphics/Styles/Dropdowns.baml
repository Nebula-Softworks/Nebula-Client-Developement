   M S B A M L   `   `   `  ����A  =Luna X, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Luna_X.CoreLuna_X.Core  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089_ [MaterialDesignThemes.Wpf, Version=4.8.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a  �� MergedDictionaries �� Source$    Luna_X.Core.RectangleConverter   RC �� 
TargetType �� Property �� Value �� 
TargetType ��Name  AccentClear  AccentStroke �� CornerRadius J� Opacity	 J� Effect-  'System.Windows.Media.Effects.BlurEffect
   Radius J� Clip �� RadiusX �� RadiusY �� Rect `� 	Converter �� Path �� Triggers �� Binding �� RelativeSource �� Value N� EnterActions g� 	BeginTime �� TargetProperty	 W� To g� Duration W� EasingFunction0  *System.Windows.Media.Animation.QuarticEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode N� ExitActions 
 Accent W� From
 � Tag �� BasedOn  �� From	! �� To" �� EasingFunction  ComboBoxToggleButton# �� AncestorType$ �� Padding% F�Name& J� RenderTransformOrigin' �� Stretch( �� Triggers) ^� 
Conditions* �� Binding+ �� Value, P� Property- P� Value. �� 
TargetName  PrimaryHueMidBrush " MaterialDesignCheckBoxDisabled/ �� ScaleY0 J� SnapsToDevicePixels1 ��Name2 J� IsHitTestVisible3 �� SelectionBoxItemTemplate4 �� SelectionBoxItem5 d�Name6 d� 	IsChecked7 �� Mode8 �Name9 �� IsDropDownOpen: � AllowsTransparency�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      "Coreclr-namespace:Luna_X.Core  5      9m1http://materialdesigninxaml.net/winfx/xaml/themes  5        5      �� 5   
   $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Shadows.xaml?�6   �� 5   
   $w qpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.ValidationErrorTemplate.xaml?�6   �� 5	   
   $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Menu.xaml?�6   �� 5
   
   $8 2/Luna X;component/Graphics/Styles/Scroll Bars.xaml?�6   �� 5   
   $3 -/Luna X;component/Graphics/Styles/Colors.xaml?�6   %�  &	        '��      7   7��7��&	 �    7  7  7  '�� �    7     5      6   �� 5      " ��6   .�5   
   ��  � ��
 ���� 6'   �� 5   
    � ��
 �����6'   �� 5   
    � ��$	 5,2q�6$   �� 5   
    � ��$ Stretch=�67   �� 5   
    � ��$ Center=�65   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      � .+�5      �� $ border��5      $��Stretch=�5      $��5 0 5 0q�5      $��36��5      #���  5      #���  5       $	��0.5q�5!      $ 4 4 4 4��5"      $ 0Q�5#      ._�5%      � .+�5&   "   �� $ TopGlow��6)   $��20��5'   )   $��20��5(   )   $��	0,-5,-4,0q�5)   )   $��Right=�5*   )   $	��Top=�5+   )   #���  5,   )   $	 200��5-   )   	 5.   &     5/   *   $
 20Q�65   51   #   �� 52   "   $ 
BottomGlow��6)   $��20��53   )   $��20��54   )   $��	-4,0,0,-5q�55   )   $
��Left=�56   )   $��Bottom=�57   )   #���  58   )   $	 200��59   )   	 5:   &     5;   *   $
 20Q�65   5=   #   5>       5@      �� 5A   "   $ 4Q�64   $ 4Q�5B   4    5C   &   `� 5D   *   8   .1�5E   .   �� $ ActualWidth�66   �� 5F   .   $ ActualHeight�66   5G   +   5I   #   ��5L      �� 5M   "    5N   &   �� 5O   *    66   �� *
IsSelected+ �� *TemplatedParent+ True6�    5P   .   �� 5Q   2   .��5R   6   �� .��5S   :   W� $ 0:0:0.2X�5T   %   $ Opacity�5U   %   $ 1X�5V   %   $ 0:0:0.2D�5W   %    5X   >     5Y   B   $ 	EaseInOut=�6N   5[   ;   5\   7   5]   3    5_   .   �� 5`   2   .��5a   6   �� .��5b   :   W� $ Opacity�5c   %   $ 0X�5d   %   $ 0:0:0.1D�5e   %    5f   >     5g   B   $ 	EaseInOut=�6N   5i   ;   5j   7   5k   3   5m   +   5p   #   5s      �� 5u      $ 	Indicator��5v      $��4��5w      $��3��5x      $��9.5,0.5,0,0q�5y      $
��Left=�5z      #���  5{      $	 1.2��5|      $ 0Q�5}      ��5~      �� 5   "    5�   &   �� 5�   *    66   �� *
IsSelected+ �� *TemplatedParent+ True6�    5�   .   �� 5�   2   .��5�   6   �� .��5�   :   W� $ 0:0:0.1X�5�   %   $ Height�5�   %   $ 4X�5�   %   $ 18X�5�   %   $ 0:0:0.3D�5�   %    5�   >     5�   B   $ 	EaseInOut=�6N   5�   ;   W� 5�   :   $ Opacity�5�   %   $ 1X�5�   %   $ 0:0:0.15D�5�   %    5�   >     5�   B   $ 	EaseInOut=�6N   5�   ;   5�   7   5�   3    5�   .   �� 5�   2   .��5�   6   �� .��5�   :   W� $ Height�5�   %   $ 18X�5�   %   $ 4X�5�   %   $ 0:0:0.3D�5�   %    5�   >     5�   B   $ 	EaseInOut=�6N   5�   ;   W� 5�   :   $ 0:0:0.2X�5�   %   $ Opacity�5�   %   $ 0X�5�   %   $ 0:0:0.15D�5�   %    5�   >     5�   B   $ 	EaseInOut=�6N   5�   ;   5�   7   5�   3   5�   +   5�   #   5�      �� 5�      $��46��6    $��36��5�       $��0,1,0,0q�5�       $��5 0 0 0q�5�       $
��Left=�5�       $��Center=�5�       $��Center=�5�       $��Center=�5�       
������ $��0q�5�       #��z 5�       #��z��5�       #��z��5�       ���	#FFD3D3D3��5�       Z� ��5�      �� 5�   "   " ��6(   8   5�   &   �� 5�   *    66   �� *
IsSelected+ �� *TemplatedParent+ True6�    5�   .   �� 5�   2   .��5�   6   �� .��5�   :   �� $  	#FFD3D3D3X�6I   $0 *(Label.Foreground).(SolidColorBrush.Color)�5�   I   $! 	#FFFFFFFFX�5�   I   $ 0:0:0.2D�5�   I   " 5�   >     5�   B   $ 	EaseInOut=�6N   5�   ;   5�   7   5�   3    5�   .   �� 5�   2   .��5�   6   �� .��5�   :   �� $  	#FFFFFFFFX�6I   $0 *(Label.Foreground).(SolidColorBrush.Color)�5�   I   $! 	#FFD3D3D3X�5�   I   $ 0:0:0.2D�5�   I   " 5�   >     5�   B   $ 	EaseInOut=�6N   5�   ;   5�   7   5�   3   5�   +   5�   #   5�      �� 5�      $��36��6    $��0,0,0,0q�5�       $��45 0 0 0q�5�       $
��Left=�5�       $��Center=�5�       $��Center=�5�       $��Center=�5�       
������ $��0q�5�       #��z��5�       $6��0/Luna X;component/Graphics/Fonts/#Poppins Medium)�5�       $��12(�5�       ���	#FFD3D3D3��5�       Z� ��5�      �� 5�   "   " ��6(   8   5�   &   �� 5   *    66   �� *
IsSelected+ �� *TemplatedParent+ True6�    5  .   �� 5  2   .��5  6   �� .��5  :   �� $  	#FFD3D3D3X�6I   $0 *(Label.Foreground).(SolidColorBrush.Color)�5  5   $! 	#FFFFFFFFX�5  5   $ 0:0:0.2D�5  5   " 5	  >     5
  B   $ 	EaseInOut=�6N   5  ;   5  7   5  3    5  .   �� 5  2   .��5  6   �� .��5  :   �� $  	#FFFFFFFFX�6I   $0 *(Label.Foreground).(SolidColorBrush.Color)�5  5   $! 	#FFD3D3D3X�5  5   $ 0:0:0.2D�5  5   " 5  >     5  B   $ 	EaseInOut=�6N   5  ;   5  7   5  3   5  +   5   #   5"     5#     5$     5&     5'     �� 5)     " d�6)   .�5*  
   ��  � ��
 ���� 6'   �� 5+  
    �@��	ClickMode$ Press=�6&   �� 5,  
    � |� .  6&   �� 5-  
    � �� 6'   ��  �� "# �$ (TextElement.Foreground)��� 5.  
    � �� .  6&   �� 5/  
    �@�OverridesDefaultStyle . 62   �� 50  
    � �� 51     �� 52     " d�6"   .R�53     � .+�54     �� $ ToggleTemplateRoot��6!   #$ z��55     #��z��56     #��z��57     #��z��58     ._�59     �� $ splitBorder��6%   $��0q�5:     $��Right=�5;     $��Center=�5<     
������ $��0q�5>     ._�5?  "   F� $% arrow��6'   $��0q�5@     $��10��5A  '   $��10��5B  '   $��Right=�5C     $��Center=�5D     +������j ��� � � ��� �@f��� ���j ��� 	#��z��5F     $& 0.5,0.5&�5G     $' Uniform=�5H     5I     5J     5K     ( 5L     ^� 5M     ) 5N     �� 5O  "   * 6,   �� *IsMouseOver+ �� *Self++ True6r   �� 5P  "   * 6,   �� *
IsEditable+ �� "# ��	+ False6�   5R     ^� 5S     ) 5T     �� 5U  "   * 6,   �� *IsMouseOver+ �� *Self++ True6r   �� 5V  "   * 6,   �� *
IsEditable+ �� "# ��+ True6�   5X     P� 5Y     , � ��- . . �5Z     �� $. arrow��6%   8    � ��6H   5[     P� 5\     , � {�- .  . �5]     �� $. arrow��6%   8   � ��6H   5^     ^� 5_     ) 5`     �� 5a  "   * 6,   �� *	IsEnabled+ �� *Self+	+ False6p   �� 5b  "   * 6,   �� *
IsEditable+ �� "# ��	+ False6�   .0�5d     �� $. ToggleTemplateRoot��6%    � ��
 ���� 6\   5e     ^� 5f     ) 5g     �� 5h  "   * 6,   �� *	IsEnabled+ �� *Self+	+ False6p   �� 5i  "   * 6,   �� *
IsEditable+ �� "# ��+ True6�   .0�5k     �� $. ToggleTemplateRoot��6%    � ��
 ���� 6\   �� 5l     $. splitBorder��6%   8   � ��6U   5m     P� 5n     , �@d�	IsChecked$
- TrueY�67   . �5o     �� $. arrow��6%    � z� 5p  "   �� 5q  &   $/ -1Q�65   5s     5t     5v     5x     5y     �� 5{     " ��6   .�5|  
   ��  � �� 5}     �� 5~     " ��6"   .R�5     � $��0 0 0 0q�6   #��z��5�     .+�5�     �� $ 
MainBorder��5�     #��z��5�     #��z��5�     #��z��5�     $ 4��5�     0 . 5�     �� 5�     $1 ContentSite��5�     2 .  #��z3 5�     #��z��5�     #��z4 5�     $��	8,0,5,0.5q�5�     $��Center=�5�     $
��Left=�5�     d� 5�     $5 toggleButton��6'   $��2��5�     $	��8 0q�5�     #��z��5�     #��z��5�     8��  6 5�     �� *IsDropDownOpen+$7 TwoWay=� �� *TemplatedParent+5�     � 5�     $8 Popup��5�     $��Bottom=�5�     $	��180��5�  !   #��z9 5�     : . |�.  $��0 15 0 0q�5�     $
��None=�5�     .��5�     �� $ PopupBorder��5�     ���	#FF1B1A1E#��z��5�     $��1q�5�     $��
0, 5, 0 ,0q�5�  !   $ 4��5�     $	��180��5�  !   0 . ._�5�  "   �� .��5�  &   �� $��0 3 0 3q�65   5�  #   5�     5�     5�     5�     5�     5�     5�     