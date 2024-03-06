import {readdirSync, readFileSync, writeFileSync} from 'fs';

const sourceFiles = new Set(readdirSync('.').filter(s => {
    return !s.startsWith('_') && !s.startsWith('.') && !s.endsWith('.mjs') && s !== 'node_modules';
}));
const renderedSourceFiles = new Set();

function titleToAnchor(title) {
    return title.toLowerCase().replace(/ /g, '-');
}

function parseFilename(filename) {
    const slug = filename.replace('.md', '');
    const title = filename.replace('.md', '').replaceAll('-', ' ');
    const url = `https://github.com/BRCMapStation/Docs/wiki/${slug}`;
    return { slug, title, url };
}

//
// Rendering
//

function sectionHeader(title) {
    return `## ${title}\n\n`;
}
function pageHeader(title, url) {
    return `### [${title}](${url})\n`;
}

function page(filename, {title, includeHeaders} = {}) {
    renderedSourceFiles.add(filename);

    let a = ``;
    const {slug, title: _title, url} = parseFilename(filename);
    title ??= _title;

    a += pageHeader(title, url);

    if(includeHeaders) {
        let markdown = '';
        try {markdown = readFileSync(filename, 'utf8');}catch {}

        markdown.replace(/^(#+) *(.*)\n/mg, (_0, poundSigns, /** @type{string} */title) => {

            const anchor = titleToAnchor(title);
            a += '  '.repeat(poundSigns.length - 1) + `- [${title}](${url}#${anchor})\n`;

        });
    }

    a += '\n';

    return a;
}

let a = '';

a += page('Home.md');
a += sectionHeader('Mechanics');
a += page('Mechanics-Overview.md', {title: 'Overview', includeHeaders: true});
a += page('Cypher.md');
a += page('Glass.md');
a += page('Graffiti-Spot.md');
a += page('Grind.md');
a += page('Skateboard-Screw-Pole.md');
a += page('Launcher.md');
a += page('Speed-Zone.md');
a += page('Sun.md');
a += page('Teleporter.md');
a += page('Vending-Machine.md');
a += page('Vert-Ramp.md');
a += page('Walk-Zone.md');
a += page('Wallrun.md');

for(const f of sourceFiles) {
    if(!renderedSourceFiles.has(f)) {
        page(f, true);
    }
}

writeFileSync('_Sidebar.md', a);