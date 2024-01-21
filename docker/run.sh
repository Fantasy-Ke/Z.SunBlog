#!/bin/bash
# 检查服务是否存在
if  docker-compose -p sunblog ps > /dev/null 2>&1; then
    # 服务存在，停止并删除
    docker compose -f docker-compose.yml -p sunblog down
    echo "compose sunblog removed."
    sleep 10
fi
sudo docker compose -f docker-compose.yml -p sunblog pull && docker compose -f docker-compose.yml -p sunblog up -d
