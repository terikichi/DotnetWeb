FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# 作業ディレクトリの設定
WORKDIR /workspace

# 必要なツールをインストール
RUN apt-get update && apt-get install -y \
    git \
    && rm -rf /var/lib/apt/lists/*

# ユーザーを変更（オプション）
CMD ["sleep", "infinity"]