# ST_kursach
Курсовая работа по СТ в АСОИУ

Два компьютера, подключенные через COM-порт, передают файлы.
Требования:
- передача идет параллельно (в две стороны одновременно);
- кодирование и декодирование информации происходит с помощью кода Хэмминга [15,11] (спасибо Алексею);
- регуляция потока должна происходить с помощью служебных пакетов или сигналов кабеля (на выбор, я предпочел служебные пакеты, но потом посмотрим);
- интерфейс, в котором можно настроить параметры (единственное, что пока готово).

Что нужно доделать:
- доделать функцию для ReadingThread - сначала выполнить декодирование, затем, в зависимости от второго бита (бита типа пакета), выполнить необходимую функцию;
- больше работы с физическим уровнем - воспользоваться командами COM-порта для более быстрой синхронизации;
- переделать функцию упаковки в пакеты;
- реализовать/переделать ещё кучу вещей в коде - см. комментарии в самом коде.

Документация:
- Расчетно-пояснительная записка.
- описание программы;
- руководство пользователя;
- программа и методика испытаний.
- Графическая часть на 3 (6) листах формата А1 (А2):
- Структурная схема программы.
- Структура протокольных блоков данных.
- Структурные схемы основных процедур взаимодействия объектов по разработанным протоколам.
- Временные диаграммы работы протоколов.
- Граф диалога пользователя.
- Алгоритмы программ.
