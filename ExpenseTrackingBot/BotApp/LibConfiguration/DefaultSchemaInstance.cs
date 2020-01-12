using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BotDesignerLib;

namespace ExpenseTrackingBot
{
    public class DefaultSchemaInstance: Schema
    {
        public DefaultSchemaInstance()
        {
            Name = "default";
            var messageBlocks = createMessageBlocks();
            var transitions = createTransitions();

            Steps = createSchemaSteps(messageBlocks, transitions);
            Blocks = messageBlocks;
        }

        private List<SchemaActionBlock> createMessageBlocks ()
        {
            var messageBlocks = new List<SchemaActionBlock>();
            
            messageBlocks.AddRange(
                new List<SchemaActionBlock> ()
                {
                    new SchemaActionBlock()
                    {
                        Name = "Onboarding",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                Content = "Привет! Этот бот поможет тебе записывать твои расходы и потом анализировать их",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Id = "2",
                                Content = "Кcтати, как тебя зовут?",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Id = "3",
                                PropertySetter = (c,i) => ((DomainDataContext)c.DataContext).UserName = i,
                                //CustomMethod = DomainActions.SaveUserName,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "4",
                                //Content = "Your name is: ", //+ ((DomainDataContext)Chat.DataContext).UserName,
                                TextWithProperties = c => $"Your name is: {((DomainDataContext)c.DataContext).UserName}",
                                Type = MessageType.sendMessage
                                //CustomMethod = DomainActions.SendUserName,
                                //Type= MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "5",
                                Content = "Давай авторизуемся у Google!",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Id = "6",
                                CustomMethod=GoogleAuthActions.AuthorizeAndGetSheetsService,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "7",
                                Content = "Скопируй ссылку на файл в GoogleSheets, где ты будешь хранить расходы.",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Id = "8",
                                CustomMethod = GoogleAuthActions.SetSpreadsheet,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "9",
                                CustomMethod = GoogleAuthActions.SendSheetsListForSelect,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "10",
                                //PropertySetter = v => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject.Category = v,
                                CustomMethod = GoogleAuthActions.SetSheet,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "11",
                                Content = "Напиши через точку с запятой категории расходов, которые будешь использовать",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Id = "12",
                                CustomMethod = ExpenseCategoryActions.BulkCreateCategories,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "13",
                                Content = "https://st.kp.yandex.net/images/film_iphone/iphone360_19373.jpg",
                                Type = MessageType.sendImage
                            }
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "Menu",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                Content = "Ты находишься в главном меню. Что будешь делать?",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "AddExpense",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                //PropertySetter = i => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject = new Expense(),
                                PropertySetter = (c,i) => ((DomainDataContext)c.DataContext).CurrentExpense = new Expense(),
                                Type = MessageType.Custom
                                //CustomMethod = ExpenseActions.SetNewCurrentExpense,
                                //Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "2",
                                Content = "Внеси сумму расхода:",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Id = "3",
                                CustomMethod = ExpenseActions.ProcessExpenseValue,
                                ErrorHandlingMessage = "Некорректная цифра, попробуйте еще раз",
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "4",
                                CustomMethod = ExpenseCategoryActions.SendExpenseCategoriesList,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "5",
                                //PropertySetter = v => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject.Category = v,
                                CustomMethod = ExpenseActions.SetExpenseCategory,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "6",
                                Content = "Расход создан. Отправляем в Google Sheets?",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "PushExpenseToGoogleSheets",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                TextWithProperties = c => $"{((DomainDataContext)c.DataContext).CurrentExpense.Category.Name} " +
                                $"- {((DomainDataContext)c.DataContext).CurrentExpense.ExpenseValue}",
                                Type = MessageType.sendMessage
                                //CustomMethod=ExpenseActions.PrintExpense,
                                //Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "2",
                                CustomMethod = GoogleSheetsActions.InsertExpenseToGoogleSheet,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "3",
                                PropertySetter = (c,i) => ((DomainDataContext)c.DataContext).Expenses.Add(
                                    ((DomainDataContext)c.DataContext).CurrentExpense),
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "4",
                                PropertySetter = (c,i) => ((DomainDataContext)c.DataContext).CurrentExpense = null,
                                Type = MessageType.Custom
                            }
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "CancelExpenseCreation",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                CustomMethod = ExpenseActions.DeleteCurrentExpense,
                                //PropertySetter = (c,i) => ((DomainDataContext)c.DataContext).CurrentExpense = null,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "2",
                                Content = "Расход отменен",
                                Type = MessageType.sendMessage
                            }
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "Settings",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                Content = "Что хочешь изменить?",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "ExpenseCategories",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                Content = "Что именно будем менять в категориях?",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "AddNewExpenseCategory",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                Content = "Введи название новой категории:",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Id = "2",
                                CustomMethod = ExpenseCategoryActions.CreateExpenseCategory,
                                Type = MessageType.saveUserInput,
                                ErrorHandlingMessage = "Такая категория уже существует"

                            },
                            new SchemaAction()
                            {
                                Id = "3",
                                Content = "Категория создана!",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "EditExpenseCategory",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                CustomMethod = ExpenseCategoryActions.SendExpenseCategoriesList,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "2",
                                CustomMethod = ExpenseCategoryActions.SelectCategory,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "3",
                                Content = "Введите новое название категории",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Id = "4",
                                CustomMethod = ExpenseCategoryActions.EditExpenseCategory,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "5",
                                Content = "Категория изменена",
                                Type = MessageType.sendMessage
                            }
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "DeleteExpenseCategory",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                CustomMethod = ExpenseCategoryActions.SendExpenseCategoriesList,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "2",
                                CustomMethod = ExpenseCategoryActions.SelectCategory,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "3",
                                CustomMethod = ExpenseCategoryActions.DeleteExpenseCategory,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "4",
                                Content = "Категория удалена",
                                Type = MessageType.sendMessage
                            }
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "GoogleSheetsIntegration",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                Content = "Что именно настроим?",
                                Type = MessageType.sendMessage
                            },
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "SetGoogleSheetId",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                Content = "Скопируй ссылку на файл в GoogleSheets, где ты будешь хранить расходы.",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Id = "2",
                                CustomMethod = GoogleAuthActions.SetSpreadsheet,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Id = "3",
                                CustomMethod = GoogleAuthActions.SendSheetsListForSelect,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "4",
                                //PropertySetter = v => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject.Category = v,
                                CustomMethod = GoogleAuthActions.SetSheet,
                                Type = MessageType.saveUserInput
                            }
                        }
                    },
                    new SchemaActionBlock()
                    {
                        Name = "GoogleAuthentication",
                        Messages = new List<SchemaAction>()
                        {
                            new SchemaAction()
                            {
                                Id = "1",
                                CustomMethod = GoogleAuthActions.RemoveGoogleAuthorization,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "2",
                                CustomMethod=GoogleAuthActions.AuthorizeAndGetSheetsService,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Id = "3",
                                Content = "Авторизация прошла успешно!",
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

        private List<SchemaStep> createSchemaSteps(List<SchemaActionBlock> messageBlocks, List<Transition> transitions)
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
