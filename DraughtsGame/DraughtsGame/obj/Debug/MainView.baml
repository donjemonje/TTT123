   M S B A M L   `   `   `  ����G  CDraughtsGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:DraughtsGameDraughtsGame  5   	       DraughtsGame.MainView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:DraughtsGame     5   	     � Title$
  Gamw��5   	   $	��356��5   	   $	��440��5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � 
ResizeMode$ NoResize=�5   	   .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $	��150 �6   .+�5   
   �� -   5         ��Name$ canvas��5      $��0��6!   $	��200��61   $	��400��6=   � 5   
   $��1��6   
�������.+�5      �� -   5         ��Name$ GroupOneNameLabel��5      $��Label��6/   $
��Left=�6?   $��	10,10,0,0q�6Z   $	��Top=�6m   $	��130��6�   �� 5      -   5        $ GroupTwoNameLabel��5      $��Label��6/   $
��Left=�6?   $��
10,221,0,0q�6Z   $	��Top=�6n   $	��130��6�   �� 5      -   5        $ currentGroupLabel��5      $��Label��6/   $
��Left=�6?   $��
10,190,0,0q�6Z   $	��Top=�6n   $	��130��6�   �� 5      -   5        $ GroupOnePlayersLabel��5      $��Label��62   $
��Left=�6B   $��	10,36,0,0q�6]   $	��Top=�6p   $	��130��6�   $	��149��6�   �� 5      -   5        $ GroupTwoPlayersLabel��5      $��Label��62   $
��Left=�6B   $��
10,252,0,0q�6]   $	��Top=�6q   $	��130��6�   $	��149��6�   5      5      5      