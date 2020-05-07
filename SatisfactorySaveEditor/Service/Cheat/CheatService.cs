using CommonServiceLocator;
using SatisfactorySaveEditor.Cheat;
using SatisfactorySaveParser.Save;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace SatisfactorySaveEditor.Service.Cheat
{
    public class CheatService
    {
        public ObservableCollection<CheatViewModel> CheatInfo { get; } = new ObservableCollection<CheatViewModel>();

        private readonly Dictionary<Type, ICheat> _cheatInstances = new Dictionary<Type, ICheat>();

        public CheatService()
        {
            var cheatTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(m => m.IsClass && m.GetInterface(nameof(ICheat)) != null);

            foreach (var cheatType in cheatTypes)
            {
                var info = cheatType.GetCustomAttribute<CheatInfoAttribute>();
                if (info == null)
                {
                    CheatInfo.Add(new CheatViewModel
                    {
                        Name = "MISSING INFO ATTRIBUTE",
                        Description = "ADD A CheatInfo ATTRIBUTE TO YOUR CHEAT CLASS",
                        Type = cheatType
                    });
                }
                else
                {
                    CheatInfo.Add(new CheatViewModel
                    {
                        Name = info.Name,
                        Description = info.Description,
                        Type = cheatType
                    });
                }
            }
        }

        /// <summary>
        /// Attempts to run a cheat by checking the CanExecute method, followed by the Execute method if the check returned true
        /// </summary>
        /// <param name="cheatType">Type of the cheat</param>
        /// <param name="saveGame">Savegame to execute the cheat on</param>
        /// <param name="selectedItem">Item selected in the right panel</param>
        /// <param name="multiSelected">Items with checkboxes in the left tree</param>
        /// <returns>Whether this cheat has been executed</returns>
        public bool RunCheat(Type cheatType, FGSaveSession saveGame, SaveObject selectedItem, List<SaveObject> multiSelected)
        {
            var cheat = _cheatInstances.GetValueOrDefault(cheatType) ?? CreateCheatInstance(cheatType);
            if (cheat.CanExecute(saveGame, selectedItem, multiSelected))
            {
                cheat.Execute(saveGame, selectedItem, multiSelected);
                return true;
            }

            return false;
        }

        private ICheat CreateCheatInstance(Type cheatType)
        {
            var constructor = cheatType.GetConstructors().First(c => c.IsPublic);
            var parameters = constructor.GetParameters();

            var services = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameter = parameters[i];
                services[i] = ServiceLocator.Current.GetInstance(parameter.ParameterType);
            }

            var cheat = (ICheat)Activator.CreateInstance(cheatType, services);
            _cheatInstances[cheatType] = cheat;

            return cheat;
        }
    }
}
