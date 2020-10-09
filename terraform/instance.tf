# define instances
resource "openstack_compute_instance_v2" "node" {
    count           = var.numNodes
    name            = "node${count.index}"
    image_id        = "${openstack_images_image_v2.rancheros.id}"
    flavor_id       = 3
# TODO change to a fixed key pair or generate it somewhere
#    key_pair        = "sjouke_openpgp"
    security_groups = ["default", "k8s-node"]
    network {
        name = "internal_vlan16"
    }
}

# create a floating IP for each instance
resource "openstack_networking_floatingip_v2" "fip" {
    count           = var.numNodes    
    pool            = "vlan16"
}

# associate the IPs with the instances
resource "openstack_compute_floatingip_associate_v2" "fip" {
    count           = var.numNodes    
    floating_ip     = "${openstack_networking_floatingip_v2.fip[count.index].address}"
    instance_id     = "${openstack_compute_instance_v2.node[count.index].id}"
}

# outputs
output "ips" {
  value           = openstack_compute_floatingip_associate_v2.fip[*].floating_ip
}