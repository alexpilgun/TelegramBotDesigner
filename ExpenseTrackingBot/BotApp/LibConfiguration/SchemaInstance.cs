using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BotDesignerLib;

namespace ExpenseTrackingBot
{
    public class SchemaInstance
    {
        public Schema schema { get; }

        public SchemaInstance()
        {
            schema = new Schema("Onboarding");
            var messageBlocks = createMessageBlocks();
            var transitions = createTransitions();

            schema.Steps = createSchemaSteps(messageBlocks, transitions);
        }

        private List<MessageBlock> createMessageBlocks ()
        {
            var messageBlocks = new List<MessageBlock>();
            
            messageBlocks.AddRange(
                new List<MessageBlock> ()
                {
                    new MessageBlock()
                    {
                        Name = "Onboarding",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Привет! Этот бот поможет тебе записывать твои расходы и потом анализировать их",
                                Type = MessageType.sendMessage
                            },
                            new Message()
                            {
                                Content = "Кcтати, как тебя зовут?",
                                Type = MessageType.sendMessage
                            },
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.saveUserInput
                            },
                            new Message()
                            {
                                CustomMethod = DomainActions.SaveUserName,
                                Type = MessageType.Custom
                            },
                            new Message()
                            {
                                Content = "Заноси свои расходы, получай агрегированные отчеты и понимай, на что именно ты тратишь деньги!",
                                Type = MessageType.sendMessage
                            },
                            new Message()
                            {
                                Content = "https://st.kp.yandex.net/images/film_iphone/iphone360_19373.jpg",
                                Type = MessageType.sendImage
                            }
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "Menu",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Ты находишься в главном меню. Что будешь делать?",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "AddExpense",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Внеси сумму расхода:",
                                Type = MessageType.sendMessage
                            },
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.saveUserInput
                            },
                            new Message()
                            {
                                Content = "Выбери категорию расхода:",
                                Type = MessageType.sendMessage
                            },
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.saveUserInput
                            },
                            new Message()
                            {
                                Content = "Ура, расход учтен!",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "PushExpenseToGoogleSheets",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.Custom
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "CancelExpenseCreation",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.Custom
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "Settings",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Что хочешь изменить?",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "ExpenseCategories",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Что именно будем менять в категориях?",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "AddNewExpenseCategory",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Введи название новой категории:",
                                Type = MessageType.sendMessage
                            },
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.saveUserInput
                            },
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.Custom
                            },
                            new Message()
                            {
                                Content = "Ура, категория создана!",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "EditExpenseCategory",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Какую категорию изменим?",
                                Type = MessageType.sendMessage
                            },
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.saveUserInput
                            },
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.Custom
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "DeleteExpenseCategory",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Какую категорию удалим?",
                                Type = MessageType.sendMessage
                            },
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.saveUserInput
                            },
                            new Message()
                            {
                                Content = "Какую категорию удалим?",
                                Type = MessageType.Custom
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "GoogleSheetsIntegration",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Что именно настроим?",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "SetGoogleSheetId",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "Пришли ID Google Sheets, где будут хранится твои расходы.",
                                Type = MessageType.sendMessage
                            },
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.saveUserInput
                            },
                            new Message()
                            {
                                Content = "Записано!",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new MessageBlock()
                    {
                        Name = "GoogleAuthentication",
                        Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Content = "",
                                Type = MessageType.Custom
                            },
                            new Message()
                            {
                                Content = "Соединение установлено!",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                }
                );


            return messageBlocks; 
        }

        private List<Transition> createTransitions()
        {
            var transitions = new List<Transition>()
            {
                new Transition()
                {
                    Name = "OnboardingToMenu",
                    DisplayName = "Погнали!",
                    IsDefault = true
                },
                new Transition()
                {
                    Name = "MenuToAddExpense",
                    DisplayName = "Новый расход",
                },
                new Transition()
                {
                    Name = "AddExpenseToPushExpenseToGoogleSheets",
                    DisplayName = "Подтвердить расход",
                },
                new Transition()
                {
                    Name = "AddExpenseToCancelExpenseCreation",
                    DisplayName = "Отменить расход",
                },
                new Transition()
                {
                    Name = "PushExpenseToGoogleSheetsToMenu",
                    DisplayName = "",
                    IsDefault = true
                },
                new Transition()
                {
                    Name = "CancelExpenseCreationToMenu",
                    DisplayName = "",
                    IsDefault = true
                },
                new Transition()
                {
                    Name = "MenuToSettings",
                    DisplayName = "Настройки",
                },
                new Transition()
                {
                    Name = "SettingsToMenu",
                    DisplayName = "В меню",
                },
                new Transition()
                {
                    Name = "SettingsToExpenseCategories",
                    DisplayName = "Категории расходов",
                },
                new Transition()
                {
                    Name = "ExpenseCategoriesToSettings",
                    DisplayName = "В настройки",
                },
                new Transition()
                {
                    Name = "ExpenseCategoriesToAddNewExpenseCategory",
                    DisplayName = "Новая категория",
                },
                new Transition()
                {
                    Name = "AddNewExpenseCategoryToExpenseCategories",
                    DisplayName = "Создать категорию",
                    IsDefault = true
                },
                new Transition()
                {
                    Name = "ExpenseCategoriesToEditExpenseCategory",
                    DisplayName = "Изменить категорию",
                },
                new Transition()
                {
                    Name = "EditExpenseCategoryToExpenseCategories",
                    DisplayName = "Подтвердить изменение",
                    IsDefault = true
                },
                new Transition()
                {
                    Name = "ExpenseCategoriesToDeleteExpenseCategory",
                    DisplayName = "Удалить категорию",
                },
                new Transition()
                {
                    Name = "DeleteExpenseCategoryToExpenseCategories",
                    DisplayName = "Подтвердить удаление",
                },
                new Transition()
                {
                    Name = "SettingsToGoogleSheetsIntegration",
                    DisplayName = "Интеграция с GoogleSheets"
                },
                new Transition()
                {
                    Name = "GoogleSheetsIntegrationToSettings",
                    DisplayName = "В настройки"
                },
                new Transition()
                {
                    Name = "GoogleSheetsIntegrationToSetGoogleSheetId",
                    DisplayName = "Выбрать лист"
                },
                new Transition()
                {
                    Name = "SetGoogleSheetIdToGoogleSheetsIntegration",
                    DisplayName = "Интеграция с GoogleSheets",
                    IsDefault = true
                },
                new Transition()
                {
                    Name = "GoogleSheetsIntegrationToGoogleAuthentication",
                    DisplayName = "Авторизация в Google Sheets"
                },
                new Transition()
                {
                    Name = "GoogleAuthenticationToGoogleSheetsIntegration",
                    DisplayName = "Интеграция с GoogleSheets",
                    IsDefault = true
                },
            };

            return transitions;
        }

        private List<SchemaStep> createSchemaSteps(List<MessageBlock> messageBlocks, List<Transition> transitions)
        {
            var steps = new List<SchemaStep>() { };
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "Onboarding").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "Menu").First(),
                Transition = transitions.Where(x => x.Name == "OnboardingToMenu").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "Menu").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "AddExpense").First(),
                Transition = transitions.Where(x => x.Name == "MenuToAddExpense").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "AddExpense").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "PushExpenseToGoogleSheets").First(),
                Transition = transitions.Where(x => x.Name == "AddExpenseToPushExpenseToGoogleSheets").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "AddExpense").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "CancelExpenseCreation").First(),
                Transition = transitions.Where(x => x.Name == "AddExpenseToCancelExpenseCreation").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "CancelExpenseCreation").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "Menu").First(),
                Transition = transitions.Where(x => x.Name == "CancelExpenseCreationToMenu").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "PushExpenseToGoogleSheets").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "Menu").First(),
                Transition = transitions.Where(x => x.Name == "PushExpenseToGoogleSheetsToMenu").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "Menu").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "Settings").First(),
                Transition = transitions.Where(x => x.Name == "MenuToSettings").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "Settings").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "Menu").First(),
                Transition = transitions.Where(x => x.Name == "SettingsToMenu").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "Settings").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "ExpenseCategories").First(),
                Transition = transitions.Where(x => x.Name == "SettingsToExpenseCategories").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "ExpenseCategories").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "Settings").First(),
                Transition = transitions.Where(x => x.Name == "ExpenseCategoriesToSettings").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "ExpenseCategories").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "AddNewExpenseCategory").First(),
                Transition = transitions.Where(x => x.Name == "ExpenseCategoriesToAddNewExpenseCategory").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "AddNewExpenseCategory").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "ExpenseCategories").First(),
                Transition = transitions.Where(x => x.Name == "AddNewExpenseCategoryToExpenseCategories").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "ExpenseCategories").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "EditExpenseCategory").First(),
                Transition = transitions.Where(x => x.Name == "ExpenseCategoriesToEditExpenseCategory").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "EditExpenseCategory").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "ExpenseCategories").First(),
                Transition = transitions.Where(x => x.Name == "EditExpenseCategoryToExpenseCategories").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "ExpenseCategories").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "DeleteExpenseCategory").First(),
                Transition = transitions.Where(x => x.Name == "ExpenseCategoriesToDeleteExpenseCategory").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "DeleteExpenseCategory").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "ExpenseCategories").First(),
                Transition = transitions.Where(x => x.Name == "DeleteExpenseCategoryToExpenseCategories").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "Settings").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "GoogleSheetsIntegration").First(),
                Transition = transitions.Where(x => x.Name == "SettingsToGoogleSheetsIntegration").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "GoogleSheetsIntegration").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "Settings").First(),
                Transition = transitions.Where(x => x.Name == "GoogleSheetsIntegrationToSettings").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "GoogleSheetsIntegration").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "SetGoogleSheetId").First(),
                Transition = transitions.Where(x => x.Name == "GoogleSheetsIntegrationToSetGoogleSheetId").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "SetGoogleSheetId").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "GoogleSheetsIntegration").First(),
                Transition = transitions.Where(x => x.Name == "SetGoogleSheetIdToGoogleSheetsIntegration").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "GoogleSheetsIntegration").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "GoogleAuthentication").First(),
                Transition = transitions.Where(x => x.Name == "GoogleSheetsIntegrationToGoogleAuthentication").First(),
            });
            steps.Add(new SchemaStep()
            {
                FromBlock = messageBlocks.Where(x => x.Name == "GoogleAuthentication").First(),
                ToBlock = messageBlocks.Where(x => x.Name == "GoogleSheetsIntegration").First(),
                Transition = transitions.Where(x => x.Name == "GoogleAuthenticationToGoogleSheetsIntegration").First(),
            });


            return steps;
        }
    }
}
