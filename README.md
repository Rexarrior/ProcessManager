# ProcessManager

Использование:
Демонстрационный интерфейс реализует следующие возможности: 
1. Поиск и отображение процессов, занимающих оперативной памяти более заданного значения, одновременно расходуя процессорного времени меньше указанного процента. Кнопка "Обновить". Задание порогов происходит при обновлении, используются значения из соответствующих полей формы. Если значения заданы верно, используются старые значения. 
2. Закрытие всех отобранных пунктом 1 процессов. Кнопка "Удалить все". Внимание: необязательно вызывать обновление (нажимать "Обновить"),  процесс обновления произойдет при вызове команды закрытия. По-умочанию используется "жесткий" способ закрытия процессов - команда kill. Интерфейс демонстрационного режима не предоставляет возможность для изменения способа закрытия процесса. 
3. Запуск приложения в режим автоматической работы. Кнопка "Запуск в фоне \!"  Окно интерфейса сворачивается в трей. Полный эквивалент ручного вызова закрытия всех отобранных процессов каждые три секунды (нажатия кнопки "Удалить все" каждые три секунды). При развертывании приложения из трея, оно ПРОДОЛЖИТ работать в автоматическом режима. Данный режим отключается повторным нажатием кнопки "Запуск в фоне \!". В случае изменения порогов памяти\процессора приложение, работающее в автоматическом режиме, будет работать по новым значениям порогов. 
4. "Запуск демона". Кнопка "Запуск демона \!" Под демоном подразумевается отдельно работающий процесс, не имеющий интерфеса и не предполагающий как  самостоятельного прекращения работы, так и како-го либо управления со стороны пользователя, за исключением его остановки (повторное нажатие   кнопки "Запуск демона \!". Демон работает аналогично автоматическому режиму и имеет фиксированные значения порогов (полностью наследует настройки приложения на момент нажатия кнопки "Запуск демона \!"). В случае закрытия приложения без остановки демона, демон продолжит работать в том же режиме. В демонстрационном режиме контроль над демоном средствами приложения будт полностью потерян в случае такого закрытия. Пожалуйста, остановите его самостоятельно, например, с помощью диспетчера задач.  Возможен заупуск одновременно нескольких демонов одновременно ( В демонстрационном режиме - посредством заупуска демонов их  нескольких вызовов приложения целиком).
Для тестирование можно использовать приложение "EatMemory.exe", которое предназначено для захвата указанного количества памяти. В следствии отсутствия иных действий, потребление ресурса процессора данным приложением стремится к 0. 
Это очень ранняя версия приложения. Возможны ошибки в любой части функционала. Покрытие тестами кода практически отстутствует. 
Have fun!
