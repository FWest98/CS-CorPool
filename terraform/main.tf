# Configure the OpenStack Provider
# use credentials from the environment variables
provider "openstack" {

}

# specify number of nodes
variable "numNodes" {
    type        = number
    default     = 2
}

# add image for our nodes
resource "openstack_images_image_v2" "rancheros" {
  name             = "RancherOS"
  image_source_url = "https://releases.rancher.com/os/latest/rancheros-openstack.img"
  container_format = "bare"
  disk_format      = "qcow2"
  visibility       = "shared"

}