   M S B A M L   `   `   `  ����G  CSchemeEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:SchemeEditorSchemeEditor  5   	   =#clr-namespace:SchemeEditor.ControlsSchemeEditor.Controls  5   	       SchemeEditor.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   &localclr-namespace:SchemeEditor  5   	   1control#clr-namespace:SchemeEditor.Controls  5   	     � Title$  SchemeEditor��5
   	   $	��450��6   $	��800��6+    � Icon$ 	/Icon.png��67   .��5      �  � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $	��200 �6   �� 5      $��* �6   .+�5   
   �� 
�������$��0��6#   $��2��60   
�������$��0,0,0,1q�6W   ._�5      �� .+�5      �� $
��Left=�6   $
��File��64   $��40��6C   ��6N   �� *FileButtonCommand+ � ContextMenu 6t   �� *ContextMenu+5      5      �� 5   
   -   5         ��Name$ ToolBox��5      $��0��5      $��1��6"   
�������$��Stretch=�5      $��Disabled=�5       $��
0, 0, 1, 0q�5!       � DataContext 5"      �� *	ToolBoxVM+��5#      �� *
Controls+ �� SelectedItem 5$      �� *SelectedControl+ �� Mode$ TwoWay=�*  $Microsoft.Xaml.Behaviors.Interaction	   Triggers	 5&      +  %Microsoft.Xaml.Behaviors.EventTrigger  5'      
   	EventName$
 	MouseMove��6!   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5(      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 6,   �� *DragStartCommand+5)      ��5,      �� 5-      .R�5.      �  � Orientation$ 
Horizontal=�6    5/      ��52      �� 53      .R�54      �� $��50��55   %   ��54   %   �� 55   %   56      59      �� 5;   
   $
��Auto=�6   $
��Auto=�6<   $��1��6_   $��1��6o   .��5<      �� -   5         ��Name$ 
MainCanvas��5<      $
��2000��5=      $
��2000��5>      
������� J� 	AllowDrop$
 True��5@       5A      �� *
CanvasVM+ � LayoutTransform 5C      �� 5D       �� ScaleX 6%   �� *ScaleX+ �� ScaleY 6?   �� *ScaleY+	 5G        5H      $
 DragOver��6%   . 5I           PassEventArgsToCommand$
 True��6T    60   �� *DragOverCommand+6T   5J        5K      $

 Drop��6%   . 5L         60   �� *DropCommand+   CommandParameter 6P   ��  �� ElementName$ 
MainCanvas��5M        5N      $
 	MouseMove��6%   . 5O        $
 True��5P   ,    5O   0   �� *MouseMoveCommand+5P   ,   5Q        5R      $
 MouseLeftButtonUp��6%   . 5S        $
 True��5T   ,    5S   0   �� *MouseLeftButtonUpCommand+5T   ,   5U        5V      $
 PreviewMouseWheel��6%   . 5W        $
 True��5X   ,    5W   0   �� *ZoomCommand+5X   ,   5Y      .+�5\      �� ��6   �� *
Controls+��5]      �� 5^      .R�5_      �� 5`      ��5c      �� 5d       �� 
TargetType" ��6    .�5e      ��  �� Property �@��Left �� Value 6<   �� *PositionOnCanvas.X+�� 5f       �@��Top 6;   �� *PositionOnCanvas.Y+5g      ��5j      �� 5k      .R�5l      �� ��6-   �� *	Control+	 5m   "     5n   &   $
 MouseLeftButtonDown��65   . 5o   *      6@   �� *MouseLeftButtonDownCommand+ 5p   4   �� $ 
MainCanvas��5q   '     5r   &   $
 MouseRightButtonDown��65   . 5s   *      6@   �� *MouseRightButtonDownCommand+5t   '     5u   &   $
 
MouseEnter��65   . 5v   *      6@   �� *MouseEnterCommand+5w   '     5x   &   $
 
MouseLeave��65   . 5y   *      6@   �� *MouseLeaveCommand+5z   '   5|      5}      5      �� 5�      ��6   �� *Connections+��5�      �� 5�      .R�5�      �� 5�      ��5�      �� 5�      .R�5�      F� 
���   �$��1��6M   ��6#   �� *	Path.Data+6M   5�      5�      5�      5�      5�      5�      