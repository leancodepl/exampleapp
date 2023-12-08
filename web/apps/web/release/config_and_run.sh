#!/bin/sh

if [ -z "$VARNAME" ]; then
    VARNAME="app_config"
fi

JSON_OUTPUT=$(env | awk -F= '
$1 ~ /^NX_/ {
    key = $1;
    gsub("^NX_[^=]*=", "");
    printf "\"%s\":\"%s\"\n", key, $0;
}' | paste -sd',')

SCRIPT_OUTPUT=$(echo "<script>var $VARNAME={$JSON_OUTPUT};</script></head>" | sed -E 's/[&]/\\&/g')

awk -v srch="</head>" -v repl="$SCRIPT_OUTPUT" '{ sub(srch,repl,$0); print $0 }' /usr/share/nginx/html/index.html >/usr/share/nginx/html/index.tmp.html
mv /usr/share/nginx/html/index.tmp.html /usr/share/nginx/html/index.html

echo "Config applied"
nginx -g "daemon off;"
