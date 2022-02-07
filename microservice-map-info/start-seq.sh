PH=$(echo 'pass1234' | docker run --rm -i datalust/seq config hash)

mkdir -p seq-data

docker run \
  --name seq \
  -d \
  --restart unless-stopped \
  -e ACCEPT_EULA=Y \
  -e SEQ_FIRSTRUN_ADMINPASSWORDHASH="$PH" \
  -v seq-data:/data \
  -p 80:80 \
  -p 5341:5341 \
  datalust/seq
