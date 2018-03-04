﻿using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using SimpleChat.Models;

namespace SimpleChat.Hubs
{
	[Authorize]
	public sealed class ChatHub : Hub<IChatHub>
	{
		public async Task Send(ChatMessageModel chatMessage)
		{
			if (chatMessage == null)
				throw new ArgumentNullException(nameof(chatMessage));

			await Clients.All.Send(chatMessage);
		}
	}
}