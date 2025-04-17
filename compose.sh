#!/bin/bash
docker compose build --build-arg INVALIDATE_CACHE=$(date +%s)
docker compose up -d
