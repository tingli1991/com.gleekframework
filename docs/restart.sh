#!/bin/bash
# 设置容器映射端口，格式为 host_port:container_port
HOST_PORT=3000
CONTAINER_PORT=3000

# 设定镜像和容器名称
IMAGE_NAME="com.gleekframework.docs"
CONTAINER_NAME="com.gleekframework.docs.container"

# 检查容器是否存在，并停止、删除容器
if [ "$(docker ps -aq -f name=^/${CONTAINER_NAME}$)" ]; then
    echo "停止容器：${CONTAINER_NAME}"
    docker stop "${CONTAINER_NAME}"

    echo "删除容器：${CONTAINER_NAME}"
    docker rm "${CONTAINER_NAME}"
fi

# 检查镜像是否存在，并删除镜像
if [ "$(docker images -q ${IMAGE_NAME})" ]; then
    echo "删除镜像：${IMAGE_NAME}"
    docker rmi "${IMAGE_NAME}"
fi

# 重新构建镜像（需要Dockerfile所在路径，假设与脚本在同一目录）
echo "重新构建镜像：${IMAGE_NAME}"
docker build -t "${IMAGE_NAME}" .

# 创建并运行新容器
echo "创建并运行新容器：${CONTAINER_NAME}，对外端口：${HOST_PORT}"
docker run -d --restart unless-stopped -v /usr/local/com.gleekframework.docs:/usr/local/com.gleekframework.docs --name "${CONTAINER_NAME}" -p "${HOST_PORT}":"${CONTAINER_PORT}" "${IMAGE_NAME}"