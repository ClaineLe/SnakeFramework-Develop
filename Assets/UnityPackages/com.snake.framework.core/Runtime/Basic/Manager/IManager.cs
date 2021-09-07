using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.snake.framework
{
    namespace runtime
    {
        public interface IManager
        {
            string mName { get; }
            void Initialization();
        }
    }
}