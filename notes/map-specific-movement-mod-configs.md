# Map-specific configs for Movement Mods

*Copied from [Discord](https://discord.com/channels/1149184966282969140/1149184966987624510/1192531521366675496)*

The map would essentially say:

It's <recommended/required/required for competitive modes> to play this map <with/without> mods <X,Y, and Z> using configurations <X mod movement configs>

I haven't designed a system for this yet, if anyone feels like writing down a proposal.

MapStation could expose an API for movement mods allowing them to export/import their own movement configuration as JSON.
This JSON is embedded in the map.
This API also tells the movement mod to enable/disable certain features based on the user's desired configuration.

For example, users might want:

I want to play this sonic map competitively, and it's literally impossible without M+.  Please enable this M+ config only on this map and disable it when I leave.

I want to play maps with my M+ config, I don't care what the map says, I'm not doing slopcrew races.

I normally have quick-turn disabled, but this map recommends it.  Please enable quick-turn while I'm on this map.

I normally use wallplant, but this platforming map forbids/discourages it.  Please temporarily disable wallplant while I'm on this map.
