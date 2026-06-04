from django.urls import path
from . import views

urlpatterns = [
    path('', views.dashboard, name='dashboard'),
    path('api/analytics/', views.api_analytics, name='api_analytics'),
    path('api/depth-profile/', views.api_depth_profile, name='api_depth_profile'),
    path('api/station/<str:station_id>/', views.api_station_detail, name='api_station_detail'),
]