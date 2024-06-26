# Eidos

Тестовое задание было выполнено с использованием кроссплатформенного фреймворка пользовательского интерфейса на основе XAML, вдохновленного WPF/UWP и распространяемого под лицензией MIT — Avalonia UI.

Ссылка на макеты Figma - https://www.figma.com/file/HUmOQwBOpQvo4mZAwozFpp/Untitled?type=design&node-id=0%3A1&mode=design&t=r96HjHuzl243axcw-1

- В верхней части окна содержится список задач, которые представляют из себя прогресс бары с этапом работы. По прогресс бару можно кликнуть, тогда он уменьшит свой размер и справа от него отобразится информация о задаче (Номер задачи) и кнопка "Удалить", которая удаляет эту задачу из списка задач;
- Ниже расположены две кнопки: "Добавить задачу (Тип А)" и "Добавить задачу (Тип B)". При нажататии на "Добавить задачу (Тип А)" добавляется задача в общий список с типом А, а при нажатии на кнопку "Добавить задачу (Тип B)" - задача с типом B. Представление задач отличается цветом. У каждой задачи по середине прогресс бара написан её этап, который описывается процентом выполнения. Время, которое требует задача на выполнение - генерируется рандомно при добавлении задачи. Результат задачи (успех или ошибка) с 50% вероятностью завершится успехом, в остальных случаях завершается ошибкой.
- Ещё ниже расположены окно лога и кнопки управления задачами.
- В окно лога пишется каждое нажатие на кнопку и завершение задачи (успешно или с ошибкой).
- Если нет никаких задач, то кнопки "Удалить все", "Остановить все", "Запустить все" полупрозрачны и не доступны для нажатия;
- Если Нет запущенных задач, то кнопка "Остановить все" полупрозрачна и недоступна для нажатия;
- Каждое нажатие на кнопку сопровождается изменением её фона и цвета текста на кнопке;
- По нажатию на кнопку "Остановить все" - приостанавливается выполнение всех задач и цвета приостановленных прогресс баров становятся оранжевыми;
- Если нажимаем на кнопку "Запустить все" и есть хотя бы одна задача в паузе - то возобновляем запаузенные задачи, если задач в паузе нет - то запускаем все задачи, которые завершены или не начаты;
- В нижней части окна есть две кнопки для фильтрации отображемого типа задач: "Тип A" и "Тип B". Выбраны могут быть одновременно два типа, а также каждый тип по отдельности.

Для запуска проекта необходимо воспользоваться файлом .SLN, чтобы открыть его напрямую через Visual Studio, либо запустить .exe файл в папке Release.
