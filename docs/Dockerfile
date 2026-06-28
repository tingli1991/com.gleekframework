# 指定基础镜像
FROM node:16

# 指定工作目录
WORKDIR /docs

# 拷贝文件到/docs 目录
COPY . /docs

# 安装 docsify-cli 工具
RUN npm install -g docsify-cli@latest

# 指定端口
# EXPOSE 3000/tcp

# 运行项目
ENTRYPOINT ["docsify", "serve", "."]