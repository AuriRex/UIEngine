import yaml
from os import walk

# Python 3
# pip install pyyaml
# extract the games animations with UTinyRipper or something similar
# and place them in a folder called "input"

t_head = "public static (string, string, float, Type)[] {varname} {{ get; private set; }} = new (string, string, float, Type)[] {{"
template = "(\"{path}\", \"{attribute}\", {value}f, typeof({typeString}))"
t_tail = "};"

build = ""


typeList = {
    "dcc896934d55ff44ba4b38bbd5440bd9": "HMUI.ImageView",
    "82345c29cffde5c419cc684b8cae8e52": "TMPro.TextMeshProUGUI",
    "f70555f144d8491a825f0804e09c671c": "???????Image?? Check In game",
    "6f3fff845c724a84eb4537c0782705c3": "TMPro.TextMeshProUGUI", # Probably? idk
    "None" : "UnityEngine.GameObject"
}

# those values are set by the user
banList = [
    "_color0.r", "_color0.g", "_color0.b",
    "_color1.r", "_color1.g", "_color1.b",
    "m_Color.r", "m_Color.g", "m_Color.b",
    "m_fontColor.r", "m_fontColor.g", "m_fontColor.b"
]


file_input_path = "./input/"

_, _, filenames = next(walk(file_input_path))


for fname in filenames:

    with open(file_input_path + fname, 'r') as stream:
        try:
            
            _,_,_, content = stream.read().split('\n', 3)

            build = t_head.format(varname="Default"+fname[:-5]) + "\n    "
            add_newline = False
            for curve in yaml.safe_load(content)['AnimationClip']['m_FloatCurves']:
                if add_newline:
                    build += ",\n    "
                add_newline = True
                #print("###########")
                _value = curve['curve']['m_Curve'][0]['value']
                _path = curve['path']
                _attribute = curve['attribute']

                _script = curve['script']
                _guid = ""
                try:
                    _guid = _script['guid']
                except Exception:
                    _guid = "None"

                try:
                    _guid = typeList[_guid]
                except Exception:
                    _guid = "None"

                if _attribute in banList:
                    add_newline = False
                    continue
                #print(path)
                #print(attribute)
                #print(value)
                build += template.format(path=_path, attribute=_attribute, value=_value, typeString=_guid)
            build += "\n" + t_tail
            print(build)

        except yaml.YAMLError as exc:
            print(exc)