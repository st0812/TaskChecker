using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    class MainViewModel : NotificationObject
    {
        private Dictionary<string, int> programDicitionary;
        private ObservableCollection<WorkingProgram> _programList;
        public ObservableCollection<WorkingProgram> ProgramList
        {
            get
            {
                return this._programList;
            }
            set
            {
                SetProperty(ref this._programList, value);
            }
        }

        private bool isIdle = true;
       
        private DelegateCommand _startCommand;

        public DelegateCommand StartCommand
        {
            get
            {
                return _startCommand = _startCommand ?? new DelegateCommand(
                    _ => {
                        isIdle = false;
                        //_startCommand = null;
                        StartCommand.RaiseCanExecuteChanged();
                        StopCommand.RaiseCanExecuteChanged();

                    },

                    _ =>
                    {
                        
                        return isIdle;

                    }
                    );
            }
        }
        private DelegateCommand _stopCommand;

        public DelegateCommand StopCommand
        {
            get
            {
                return _stopCommand = _stopCommand?? new DelegateCommand(
                    _ => {
                        //_stopCommand = null;
                        isIdle = true;
                        StartCommand.RaiseCanExecuteChanged();
                        StopCommand.RaiseCanExecuteChanged();

                    },

                    _ =>
                    {

                        return !isIdle;

                    }
                    );
            }
        }

        private System.Timers.Timer timer;
        public MainViewModel()
        {
            programDicitionary = new Dictionary<string, int>();
            ProgramList = new ObservableCollection<WorkingProgram>();
            //ProgramList.Add (new WorkingProgram("aaa", 13));
            BindingOperations.EnableCollectionSynchronization(ProgramList, new object());
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(updateProgramList);
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Enabled = true;
            

        }
        private void updateProgramList(object sender, ElapsedEventArgs e)
        {
            if (isIdle)
            {
                var programName = "*rest";
                var isNewProgram = true;
                foreach (var program in ProgramList)
                {
                    if (program.ProgramName.Equals(programName))
                    {
                        program.Count += 1;
                        isNewProgram = false;
                        break;
                    }
                }
                if (isNewProgram)
                {
                    ProgramList.Add(new WorkingProgram(programName, 1));
                }
            }
            else
            {
                var programName = ActiveProgramMonitor.GetActiveProgramName();
                if (programName != null)
                {
                    var isNewProgram = true;
                    foreach (var program in ProgramList)
                    {
                        if (program.ProgramName.Equals(programName))
                        {
                            program.Count += 1;
                            isNewProgram = false;
                            break;
                        }
                    }
                    if (isNewProgram)
                    {
                        ProgramList.Add(new WorkingProgram(programName, 1));
                    }
                }
            }
           
        }
        public class WorkingProgram : NotificationObject
        {
            private string _programName;
            public string ProgramName
            {
                set
                {
                    SetProperty(ref this._programName, value);
                }
                get
                {
                    return this._programName;
                }
            }
            private int _count;
            public int Count
            {
                set
                {
                    SetProperty(ref this._count, value);
                }
                get
                {
                    return this._count;
                }
            }
            

            public WorkingProgram(string programName,  int count)
            {
                this.ProgramName = programName;
                this.Count = count;
            }
        }

    }
   
}
