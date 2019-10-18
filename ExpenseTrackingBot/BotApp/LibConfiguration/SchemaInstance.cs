﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BotDesignerLib;

namespace ExpenseTrackingBot
{
    public class SchemaInstance: Schema
    {
        public SchemaInstance()
        {
            var messageBlocks = createMessageBlocks();
            var transitions = createTransitions();

            Steps = createSchemaSteps(messageBlocks, transitions);
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
                                Content = "Привет! Этот бот поможет тебе записывать твои расходы и потом анализировать их",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                Content = "Кcтати, как тебя зовут?",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                PropertySetter = i => ((DomainDataContext)Chat.DataContext).UserName = i,
                                //CustomMethod = DomainActions.SaveUserName,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                //Content = "Your name is: ", //+ ((DomainDataContext)Chat.DataContext).UserName,
                                TextWithProperties = v => "Your name is: " + ((DomainDataContext)Chat.DataContext).UserName,
                                Type = MessageType.sendMessage
                                //CustomMethod = DomainActions.SendUserName,
                                //Type= MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Content = "Давай авторизуемся у Google!",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                CustomMethod=GoogleAuthActions.AuthorizeAndGetSheetsService,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Content = "Скопируй ссылку на файл в GoogleSheets, где ты будешь хранить расходы.",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                CustomMethod = GoogleAuthActions.SetSpreadsheet,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                CustomMethod = GoogleAuthActions.SendSheetsListForSelect,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                //PropertySetter = v => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject.Category = v,
                                CustomMethod = GoogleAuthActions.SetSheet,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Content = "Напиши через точку с запятой категории расходов, которые будешь использовать",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                CustomMethod = ExpenseCategoryActions.BulkCreateCategories,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
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
                                //PropertySetter = i => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject = new Expense(),
                                PropertySetter = i => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject = new Expense(),
                                Type = MessageType.Custom
                                //CustomMethod = ExpenseActions.SetNewCurrentExpense,
                                //Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                Content = "Внеси сумму расхода:",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                CustomMethod = ExpenseActions.ProcessExpenseValue,
                                ErrorHandlingMessage = "Некорректная цифра, попробуйте еще раз",
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                CustomMethod = ExpenseCategoryActions.SendExpenseCategoriesList,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                //PropertySetter = v => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject.Category = v,
                                CustomMethod = ExpenseActions.SetExpenseCategory,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
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
                                TextWithProperties = v => String.Format("{0} - {1}",
            ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject.Category.Name,
            ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject.ExpenseValue
            ),
                                Type = MessageType.sendMessage
                                //CustomMethod=ExpenseActions.PrintExpense,
                                //Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                CustomMethod = GoogleSheetsActions.InsertExpenseToGoogleSheet,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                PropertySetter = v => ((DomainDataContext)Chat.DataContext).Expenses.Objects.Add(
                                    ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject),
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                PropertySetter = v => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject = null,
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
                                PropertySetter = v => ((DomainDataContext)Chat.DataContext).Expenses.CurrentObject = null,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
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
                                Content = "Введи название новой категории:",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                CustomMethod = ExpenseCategoryActions.CreateExpenseCategory,
                                Type = MessageType.saveUserInput,
                                ErrorHandlingMessage = "Такая категория уже существует"

                            },
                            new SchemaAction()
                            {
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
                                CustomMethod = ExpenseCategoryActions.SendExpenseCategoriesList,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                CustomMethod = ExpenseCategoryActions.SelectCategory,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                Content = "Введите новое название категории",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                CustomMethod = ExpenseCategoryActions.EditExpenseCategory,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
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
                                CustomMethod = ExpenseCategoryActions.SendExpenseCategoriesList,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                CustomMethod = ExpenseCategoryActions.SelectCategory,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                CustomMethod = ExpenseCategoryActions.DeleteExpenseCategory,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
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
                                Content = "Скопируй ссылку на файл в GoogleSheets, где ты будешь хранить расходы.",
                                Type = MessageType.sendMessage
                            },
                            new SchemaAction()
                            {
                                CustomMethod = GoogleAuthActions.SetSpreadsheet,
                                Type = MessageType.saveUserInput
                            },
                            new SchemaAction()
                            {
                                CustomMethod = GoogleAuthActions.SendSheetsListForSelect,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
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
                                CustomMethod = GoogleAuthActions.RemoveGoogleAuthorization,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
                                CustomMethod=GoogleAuthActions.AuthorizeAndGetSheetsService,
                                Type = MessageType.Custom
                            },
                            new SchemaAction()
                            {
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
