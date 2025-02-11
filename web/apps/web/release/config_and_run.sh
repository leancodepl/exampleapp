#!/bin/sh

FILE="/usr/share/nginx/html/index.html"

if [ -z "$VARNAME" ]; then
    VARNAME="app_config";
fi

JSON_OUTPUT=$(env | awk -F= '
$1 ~ /^NX_/ {
    key = $1;
    gsub("^NX_[^=]*=", "");
    printf "\"%s\":\"%s\"\n", key, $0;
}' | paste -sd',')

HEAD=$(echo "<script>var $VARNAME={$JSON_OUTPUT};</script>" | sed -E 's/[&]/\\&/g')
BODY=""

if [ -n "${NX_GTM_ID}" ]; then
    HEAD="${HEAD}<script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src='https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);})(window,document,'script','dataLayer','${NX_GTM_ID}');</script>"
    BODY='<noscript><iframe src="https://www.googletagmanager.com/ns.html?id='${NX_GTM_ID}'" height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>'
fi

awk -v srch="</head>" -v repl="$HEAD</head>" '{ sub(srch,repl,$0); print $0 }' $FILE |
awk -v srch="<body>" -v repl="<body>$BODY" '{ sub(srch,repl,$0); print $0 }' > "$FILE.tmp"

mv "$FILE.tmp" "$FILE"

echo "Config applied"
nginx -g "daemon off;"
