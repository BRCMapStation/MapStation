# Tell git to do a sparse checkout for the MappingToolkit submodule.
# MappingToolkit is a complete Unity project but we only want to get its Assets directory.
# This configs a command to run every time the submodule updates.

# To repair a broken submodule:
#     1. rm Winterland.Editor/Assets/MappingToolkit
#     2. Run this script to set config
#     3. git submodule update --init

git config --add submodule.Winterland.Editor/Assets/MappingToolkit.update "!git sparse-checkout set .gitignore .gitattributes Assets && git sparse-checkout init && git checkout"
