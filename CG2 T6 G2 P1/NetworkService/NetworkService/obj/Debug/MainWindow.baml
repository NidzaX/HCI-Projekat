   M S B A M L   `   `   `  ????I  ENetworkService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:NetworkServiceNetworkService  5      C&clr-namespace:NetworkService.ViewModelNetworkService.ViewModel  5   	   ;"clr-namespace:NetworkService.ViewsNetworkService.Views  5	   	       NetworkService.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:NetworkService     5      <	viewModel&clr-namespace:NetworkService.ViewModel     5   	   4views"clr-namespace:NetworkService.Views     5	   	     ? Title$  
MainWindow??5   	   $	??800??6   $	??750??6)    ? 
ResizeMode$ NoResize=?65    ? DataContext 5      2  ,NetworkService.ViewModel.MainWindowViewModel  5   
   6*    ? 	Resources 5       ?? DataType3   -NetworkService.ViewModel.NetworkViewViewModel*   $NetworkService.Views.NetworkViewView3   -NetworkService.ViewModel.NetworkDataViewModel*   $NetworkService.Views.NetworkDataView1   +NetworkService.ViewModel.DataChartViewModel(   "NetworkService.Views.DataChartView??%?   (??       5      *, +)(?? 2     5      *, +)(?? d     5      *, +)?? 5   
   "  6   .R?5        5      ?? 5   
   "  6   .R?5        5      ?? 5   
   "  6   .R?5        5      5      .??5$      ? .+?5%   
   ? $	??780??6   $	??Top=?6   
???????$??10,0,10,-133q?6G   .+?5&      ?? $??Center=?6   $	??Top=?62   $??0 10 0 0q?6J   $??22(?6\   
???????$
??Bold"?6}   5'      ]? 5)   
   $??0??6   
???????$??39??68   $	??Top=?6D   $??162,10,153,0q?6\   .+?5*      o? $??Right=?6   $	??Top=?6*   $??Center=?6B   $??0 0 0 0q?6_   
???????.@?5+      m? $??3 5 0 0q?6   
???????$?? Network Data View??6>   $??30??6Z   $	??130??6f    m? CommandParameter$ NetworkData??6?   $??12(?6?   
???   ? m? Command 6r   ?? *
NavCommand+6?   m? 5,      $??3 5 0 0q?6   
???????$??Network View View??6?   $??30??6Z   $	??130??6f   $ NetworkView??6?   $??12(?6?   
???   ? 6r   ?? *
NavCommand+6?   m? 5-      $??3 5 0 0q?6   
???????$??Data Chart View??6>   $??30??6W   $	??130??6c   $ 	DataChart??6?   $??12(?6?   
???   ? 6o   ?? *
NavCommand+6?   5.      5/      ?? 50   
   $??'pomoc' za listu komandi??6   $??14(?63   $??Black"?6A   
???   ?$
??Left=?6h   $	??Top=?6?   $??291,568,0,0q?6?   $	??205??6?   ?? 52   
   -   5         ??Name$
 Term??52      $??14(?6    
???????
???????$??Right=?6S   $	??148??6o    ?? TextWrapping$
 Wrap=?6|   $	??Top=?6?   $	??681??6?   $??
0,602,33,0q?6?   ??6?   ?? *	Komanda+	 ?? Mode$	 TwoWay=?.??6?   
 J? InputBindings
 53      ?? 54       ?? 	Modifiers$  f?6   
 ?? Key$ Enter??55       ?? CommandParameter 6   ??  ?? ElementName$
 Term?? ?? Command 56      ?? *
terminal+.??57   %   58      ? 59   
   $??0,2,0,0q?6   .+?5:      ?? $??10,59,10,205q?6B   ??6   ?? *CurrentViewModel+6B   5;      5<      5>      