using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AoC_2020.Day8
{
    public class Solution
    {
        public void Run()
        {

            var input = MultiLineInputReader.ReadInputAsync<string>("Day8/Input.txt").Result;
            var commands = input.Select(ConvertStringToCommand).ToArray();

            Console.WriteLine("Part 1:");
            var part1ExecutionResult = RunProgram(commands);
            LogResult(part1ExecutionResult);

            Console.WriteLine("\r\nPart 2:");
            foreach (Command[] mutatedCommands in GetPossibleProgramsEnumeration(input))
            {
                var part2ExecutionResult = RunProgram(mutatedCommands);

                // Stop once a program that exits successfully is discovered
                if (part2ExecutionResult.ExitCode == 0)
                {
                    LogResult(part2ExecutionResult);
                    return;
                }
            }
        }

        private Command ConvertStringToCommand(string s)
        {
            var commandName = s.Split(" ")[0];
            var commandAttribute = s.Split(" ")[1];
            var attributeAsInt = int.Parse(commandAttribute);

            return commandName switch
            {
                "nop" => new NoOpCommand(),
                "acc" => new AccumulateCommand() {Amount = attributeAsInt},
                "jmp" => new JumpCommand() {Jump = attributeAsInt},
                _ => throw new Exception($"Unexpected command: {commandAttribute}")
            };
        }

        private MachineState RunProgram(Command[] commands)
        {
            var machineState = new MachineState(commands);
            var programExecutor = new ProgramExecutor(machineState);

            machineState.ExitCode = programExecutor.Run();

            return machineState;
        }

        private void LogResult(MachineState machineState)
        {
            if (machineState.ExitCode == 0)
            {
                Console.WriteLine($"Program exited normally");
            }

            if (machineState.ExitCode == -1)
            {
                Console.WriteLine($"Infinite loop detected!");
            }

            Console.WriteLine($"Accumulator: {machineState.Accumulator}");
        }

        private IEnumerable<Command[]> GetPossibleProgramsEnumeration(List<string> input)
        {
            for (var i = 0; i < input.Count; i++)
            {
                var foundCommand = input[i];
                if (input[i].Contains("nop"))
                {
                    input[i] = input[i].Replace("nop", "jmp");

                }
                else if (input[i].Contains("jmp"))
                {
                    input[i] = input[i].Replace("jmp", "nop");
                }
                else
                {
                    continue;
                }

                // todo: swap out the command on the command list to improve performance instead of converting every time
                yield return input.Select(ConvertStringToCommand).ToArray();
                
                // Set the command back to what it was before
                input[i] = foundCommand;
            }
        }
    }

    public abstract class Command
    {
        public abstract void Execute(ProgramExecutor visitor);
    }

    public class JumpCommand : Command
    {
        public int Jump { get; set; }
        public override void Execute(ProgramExecutor visitor)
        {
            visitor.ExecuteJump(this);
        }
    }

    public class AccumulateCommand : Command
    {
        public int Amount { get; set; }
        public override void Execute(ProgramExecutor visitor)
        {
            visitor.ExecuteAccumulate(this);
        }
    }

    public class NoOpCommand : Command
    {
        public override void Execute(ProgramExecutor visitor)
        {
            visitor.ExecuteNoOp(this);
        }
    }

    public class MachineState
    {
        public int ExitCode { get; set; }
        public int Accumulator { get; set; }
        public int ExecutionPointer { get; set; }
        public bool[] CommandExecutionRecord { get; set; }
        public Command[] Program { get; set; }

        public MachineState(Command[] program)
        {
            Accumulator = 0;
            ExecutionPointer = 0;
            Program = program;
            CommandExecutionRecord = new bool[program.Length];
        }
    }

    public class ProgramExecutor
    {
        private readonly MachineState _state;

        public ProgramExecutor(MachineState state)
        {
            _state = state;
        }

        public int Run()
        {
            while (IsValidExecutionPointer() && !IsTargetingAlreadyExecutedCommand())
            {
                RecordCommandExecuted();
                _state.Program[_state.ExecutionPointer].Execute(this);
            }

            if(!IsValidExecutionPointer())
            {
                return 0;
            }

            if (IsTargetingAlreadyExecutedCommand())
            {
                return -1;
            }

            return 5;
        }

        public void ExecuteJump(JumpCommand command)
        {
            DoJump(command.Jump);
        }

        public void ExecuteAccumulate(AccumulateCommand command)
        {
            _state.Accumulator += command.Amount;
            DoJump(1);
        }

        public void ExecuteNoOp(NoOpCommand command)
        {
            DoJump(1);
        }

        private void DoJump(int jumpAmountRelative)
        {
            _state.ExecutionPointer += jumpAmountRelative;
        }

        private bool IsValidExecutionPointer()
        {
            return (_state.ExecutionPointer >= 0) && 
                   (_state.ExecutionPointer < _state.Program.Length);
        }

        private bool IsTargetingAlreadyExecutedCommand()
        {
            return _state.CommandExecutionRecord[_state.ExecutionPointer];
        }

        private void RecordCommandExecuted()
        {
            _state.CommandExecutionRecord[_state.ExecutionPointer] = true;
        }
    }
}
