using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public interface IRecordAudio
    {
        Task CreateAudioGraph();
        void RecordOrStop(int value);
        void OpenAudioFilePath();

    }
}
