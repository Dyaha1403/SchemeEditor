   M S B A M L   `   `   `  ����G  CSchemeEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:SchemeEditor.ViewsSchemeEditor.Views  5   	   +    %SchemeEditor.Views.ChangeSchemeWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   ,local clr-namespace:SchemeEditor.Views  5   	   $	��200��5      $	��300��6'     � WindowStyle$
  None=�63    � 
ResizeMode$ NoResize=�5	   	    � WindowStartupLocation$ CenterScreen=�6    � Icon$ 	\Icon.png��6D   .��5
      � 
������� � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      .+�5   
   � $��0��6   
�������.+�5      �� $��	/Icon.png��6   $��20��6'   $
��Left=�63   $��
1, 0, 0, 0q�6N   �� 5      $��20��6   $��20��6    $��Right=�6,   
������ $��0q�6a   $��X��6u   ��5      �� *CloseWindowCommand+��6<   ��  �� RelativeSource ��  �� AncestorType
 Window5      �� 5   
   $��1��6   $��Center=�6"   $��Center=�6?   .+�5      �� $	��200��6   ��6#   �� *DisplaySchemes+ �� SelectedIndex 6J   �� *SelectedIndex+*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5      +  %Microsoft.Xaml.Behaviors.EventTrigger  5      	   	EventName$	 SelectionChanged��6%   *  $Microsoft.Xaml.Behaviors.TriggerBase
   Actions.
 5      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 60   �� *SelectionChangedCommand+5      5      �� 5      $��20��6   $��0, 20, 0, 0q�6>   ��6"   �� *
NameScheme+6>   �� 5       �� Orientation$ 
Horizontal=�6   .+�5       �� $��Open/Create��6   $	��100��6/   $��30��6;   $��0,20,0,0q�6G   ��5!      �� *OpenCreateCommand+��6?   ��  �� 
 Window�� 5"      $��Delete��6   $	��100��6*   $��30��66   $��0, 20, 0, 0q�6B   ��5#      �� *DeleteCommand+5$      5%      5&      5'      