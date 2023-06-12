apiVersion: batch/v1
kind: Job
metadata:
  name: exampleapp-migrations
  namespace: ${NAMESPACE}
  labels:
    project: exampleapp
    component: migrations
spec:
  backoffLimit: 2
  template:
    metadata:
      labels:
        project: exampleapp
        component: migrations
        aadpodidbinding: exampleapp-migrations
    spec:
      restartPolicy: OnFailure
      containers:
        - name: migrations
          image: leancode.azurecr.io/exampleapp-migrations:${APP_VERSION}
          resources:
            requests:
              cpu: 50m
              memory: 200Mi
          env:
            - name: PostgreSQL__ConnectionString
              valueFrom:
                secretKeyRef:
                  name: exampleapp-migrations
                  key: PostgreSQL__ConnectionString
