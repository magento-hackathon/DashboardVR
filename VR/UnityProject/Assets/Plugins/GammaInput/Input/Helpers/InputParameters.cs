using UnityEngine;
using System;
using System.Collections;

namespace GammaInput {
	public class InputParameters {

		public int inputIndex { get; protected set;}

		public Vector2 startPosition { get; protected set; }
		public DateTime startTime { get; protected set; }

		public Vector2 position { get; protected set; }
		public DateTime time { get; protected set; }

		public InputParameters(int inputIndex, Vector2 position, DateTime time)
		{
			this.inputIndex = inputIndex;

			this.startPosition = position;
			this.startTime = time;

			this.position = position;
			this.time = time;
		}

		public InputParameters(int inputIndex, Vector2 position) : this(inputIndex, position, DateTime.Now)
		{

		}

		public void UpdatePosition(Vector2 position, DateTime time)
		{
			this.position = position;
			this.time = time;
		}

		public void UpdatePosition(Vector2 position)
		{
			UpdatePosition(position, DateTime.Now);
		}

		public bool InputStartSince(TimeSpan timespan)
		{
			return (startTime + timespan) >= DateTime.Now;
		}
	}
}
