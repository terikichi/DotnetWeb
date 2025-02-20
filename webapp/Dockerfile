FROM node:18-bullseye

# 作業ディレクトリの設定
WORKDIR /workspace

# 必要なツールをインストール
RUN apt-get update && apt-get install -y \
    git \
    && rm -rf /var/lib/apt/lists/*

# デフォルトで Vite をインストール
RUN npm install -g vite

# ユーザーを変更（オプション）
CMD ["sleep", "infinity"]
