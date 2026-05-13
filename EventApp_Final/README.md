# EventApp — Приложение для организации мероприятий (WPF + MVVM)

## Структура проекта

```
EventApp/
├── Models/
│   ├── Event.cs            — Модель мероприятия (INotifyPropertyChanged)
│   └── Participant.cs      — Модель участника
├── ViewModels/
│   └── MainViewModel.cs    — Главная ViewModel (MVVM)
├── Views/
│   ├── EventEditDialog.xaml(.cs)       — Диалог создания/редактирования мероприятия
│   └── ParticipantEditDialog.xaml(.cs) — Диалог регистрации/редактирования участника
├── Commands/
│   └── RelayCommand.cs     — Реализация ICommand
├── Converters/
│   └── Converters.cs       — BoolToVisibility, NullToVisibility, CountToString
├── MainWindow.xaml(.cs)    — Главное окно
├── App.xaml(.cs)
└── EventApp.csproj         — .NET 8 WPF проект
```

## Реализованные требования

### Архитектура MVVM
- `MainViewModel` реализует всю бизнес-логику
- `RelayCommand` — универсальная реализация `ICommand`
- Модели реализуют `INotifyPropertyChanged`

### 1. DataBinding
- **TwoWay**: редактирование названия, даты, места мероприятия прямо в карточке
- **TwoWay**: поля форм EventEditDialog и ParticipantEditDialog
- **OneWay**: счётчик участников `ParticipantCount` в карточке и в списке

### 2. ObservableCollection
- `ObservableCollection<Event>` — список мероприятий
- `ObservableCollection<Participant>` внутри каждого `Event`
- Автоматическое обновление счётчика участников через `CollectionChanged`

### 3. DataTemplates & ControlTemplates
- **DataTemplate `EventItemTemplate`** — карточка мероприятия в ListBox (название, дата, локация, счётчик)
- **DataTemplate `ParticipantItemTemplate`** — строка участника с аватаром, именем, email, секцией
- **ControlTemplate `RegisterButtonTemplate`** — кастомная кнопка «Зарегистрировать» с иконкой человека
- **ControlTemplate `ToolbarButtonTemplate`** — кнопки панели инструментов с эффектами hover
- Пустое состояние списка участников через `ControlTemplate` в стиле ListBox

### Команды
| Команда | Описание |
|---|---|
| `CreateEventCommand` | Создать мероприятие (диалог) |
| `EditEventCommand` | Редактировать выбранное мероприятие |
| `DeleteEventCommand` | Удалить (с подтверждением MessageBox) |
| `AddParticipantCommand` | Зарегистрировать участника |
| `EditParticipantCommand` | Редактировать участника |
| `DeleteParticipantCommand` | Удалить участника (с подтверждением) |
| `AboutCommand` | О программе |
| `ExitCommand` | Выход |

### Горячие клавиши
| Клавиша | Действие |
|---|---|
| `Ctrl+N` | Создать мероприятие |
| `Ctrl+E` | Редактировать мероприятие |
| `Ctrl+D` | Удалить мероприятие |

### Меню
- **Файл** — Новое мероприятие, Выход
- **Мероприятия** — Создать, Редактировать, Удалить
- **Участники** — Зарегистрировать, Редактировать, Удалить
- **Настройки** — Параметры
- **Помощь** — О программе

## Сборка и запуск

Требуется **.NET 8 SDK**:

```bash
cd EventApp
dotnet run
```

Или открыть `EventApp.csproj` в Visual Studio 2022.
