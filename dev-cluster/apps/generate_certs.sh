#!/bin/sh

set -e

if command -v apk 2>/dev/null
then
    apk add --no-cache openssl
fi

NAME=local.lncd.pl
SUBJ="
C=PL
O=LeanCode DEV
commonName=*.$NAME
organizationalUnitName=LeanCode DEV
emailAddress=admin@leancode.pl"
SUBJ="$(echo "$SUBJ" | tr '\n' '/')"
PASSWD=Passwd1!

if ! test -f CA.key || ! test -f CA.pem
then
    openssl genrsa -des3 -out CA.key -passout pass:"$PASSWD" 2048
    openssl req -x509 -new -nodes -subj "$SUBJ" -key CA.key -passin pass:"$PASSWD" -sha256 -days 825 -out CA.pem
fi

if ! test -f "$NAME.key" || ! test -f "$NAME.cert"
then
    openssl genrsa -out "$NAME.key" 2048
    openssl req -new -subj "$SUBJ" -key "$NAME.key" -out "$NAME.csr"
    cat > "$NAME.ext" <<-EOF
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names
[alt_names]
DNS.1 = $NAME # Be sure to include the domain name here because Common Name is not so commonly honoured by itself
DNS.2 = *.$NAME # Optionally, add additional domains (I've added a subdomain here)
EOF

    openssl x509 -req -in "$NAME.csr" -CA CA.pem -CAkey CA.key -CAcreateserial \
        -out "$NAME.cert" -days 825 -sha256 -extfile "$NAME.ext" -passin pass:"$PASSWD"
fi

chmod 666 -- CA.key CA.pem "$NAME.key" "$NAME.cert" || true
rm -f -- "$NAME.ext" "$NAME.csr" CA.srl
