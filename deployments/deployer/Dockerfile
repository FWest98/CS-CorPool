# escape=`
### Dockerfile for the deployment container
FROM quay.io/roboll/helmfile:helm3-v0.130.1 as deployer

# Install curl and gpg
RUN apk -U add --no-cache curl gnupg

# Insert sops
COPY sops-v3.6.0.linux /usr/local/bin/sops
RUN chmod 0755 /usr/local/bin/sops